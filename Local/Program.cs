using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Local
{
    class Program
    {
        static ushort serverPort;

        static string hostIp;
        static ushort hostPort;
        static Mutex serverPortMutex = new Mutex();

        static Socket hostSocket;
        static Thread hostThread;

        static List<Thread> connectionThreads = new List<Thread>();

        static bool THREAD_STOP_FLAG = false;
        static bool CONNECTION_STOP_FLAG = false;

        static Mutex consoleMutex = new Mutex();

        static void Main(string[] args)
        {
            GetServerPort();
            GetHostAddress();

            Console.WriteLine("Connecting to host..");
            if (!ConnectToHost(true))
            {
                return;
            }

            // Start connection thread
            hostThread = new Thread(() => WaitForConnections());
            hostThread.Start();

            // Change local server port
            while (true)
            {
                lock (consoleMutex)
                {
                    if (Console.KeyAvailable)
                    {
                        ConsoleKey key = Console.ReadKey(true).Key;
                        if (key == ConsoleKey.Enter)
                        {
                            GetServerPort();
                        }
                        else if (key == ConsoleKey.Escape)
                        {
                            break;
                        }
                    }
                    else
                    {
                        Thread.Sleep(10);
                    }
                }
            }

            CONNECTION_STOP_FLAG = true;
            hostThread.Join();
            hostSocket.Close();

            // Stop all threads
            THREAD_STOP_FLAG = true;
            foreach (var thread in connectionThreads)
            {
                thread.Join();
            }
        }

        static void GetHostAddress()
        {
            Console.Write("Enter ip and port of remote server: ");
            string address = Console.ReadLine();
            string[] parts = address.Split(' ');
            if (parts.Length != 2)
            {
                Console.WriteLine("Bad format");
                return;
            }
            hostIp = parts[0];
            if (!ushort.TryParse(parts[1], out hostPort))
            {
                Console.WriteLine("Bad port");
                return;
            }
        }

        static void GetServerPort()
        {
            Console.Write("Enter local server port: ");
            string serverPortStr = Console.ReadLine();
            ushort newServerPort;
            if (!ushort.TryParse(serverPortStr, out newServerPort))
            {
                Console.WriteLine("Bad port");
                return;
            }

            lock (serverPortMutex)
            {
                serverPort = newServerPort;
            }
        }

        static bool ConnectToHost(bool manual = false)
        {
            IPAddress ipAddr = IPAddress.Parse(hostIp);
            IPEndPoint localEndPoint = new IPEndPoint(ipAddr, hostPort);

            if (hostSocket != null)
            {
                if (hostSocket.Connected)
                {
                    try
                    {
                        hostSocket.Disconnect(false);
                    }
                    catch (Exception) { }
                }
            }

            int attempts = 0;
            while (true)
            {
                hostSocket = new Socket(ipAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                IAsyncResult result = hostSocket.ConnectAsync(localEndPoint);
                bool success = result.AsyncWaitHandle.WaitOne(5000);

                if (!hostSocket.Connected)
                {
                    if (manual)
                    {
                        Console.WriteLine("Connection to host timed out, retry? (y/n)");
                        lock (consoleMutex)
                        {
                            while (!Console.KeyAvailable)
                            {
                                Thread.Sleep(10);
                            }
                            if (Console.ReadKey().KeyChar == 'y')
                            {
                                Console.WriteLine("Retrying..");
                                continue;
                            }
                            else if (Console.ReadKey().KeyChar == 'n')
                            {
                                Console.WriteLine("Aborting.");
                                return false;
                            }
                        }
                    }
                    else
                    {
                        attempts++;
                        if (attempts == 9)
                        {
                            manual = true;
                        }
                        Console.WriteLine("Connection to host timed out, retrying..");
                        continue;
                    }
                }

                hostSocket.Blocking = false;
                return true;
            }
        }

        static void WaitForConnections()
        {
            try
            {
                Console.WriteLine("Waiting for connection requests..");

                Stopwatch timer = new Stopwatch();
                timer.Start();

                byte[] buffer = new byte[1];
                while (!CONNECTION_STOP_FLAG)
                {
                    // If timer exceeds 10 seconds, restart the connection
                    if (timer.ElapsedMilliseconds > 10000)
                    {
                        Console.WriteLine("Host notification not received, reconnecting..");
                        ConnectToHost();
                        timer.Restart();
                    }

                    int received;
                    try
                    {
                        received = hostSocket.Receive(buffer);
                    }
                    catch (SocketException ex)
                    {
                        if (ex.SocketErrorCode == SocketError.WouldBlock)
                        {
                            Thread.Sleep(10);
                            continue;
                        }
                        Console.WriteLine($"Host socket error {ex.SocketErrorCode}, reconnecting..");
                        ConnectToHost();
                        timer.Restart();
                        continue;
                    }
                    if (received == 0)
                    {
                        Console.WriteLine("Host socket closed.");
                        ConnectToHost(true);
                        timer.Restart();
                        continue;
                    }

                    if (buffer[0] == 127)
                    {
                        Console.WriteLine("Connection request received.");

                        // Connect to host
                        IPAddress ipAddr = IPAddress.Parse(hostIp);
                        IPEndPoint localEndPoint = new IPEndPoint(ipAddr, hostPort);

                        Socket newHostSocket = new Socket(ipAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                        IAsyncResult result = newHostSocket.ConnectAsync(localEndPoint);
                        bool success = result.AsyncWaitHandle.WaitOne(5000);

                        if (!newHostSocket.Connected)
                        {
                            Console.WriteLine("Connection timed out..");
                            continue;
                        }

                        // Start connection thread
                        Thread connectionThread = new Thread(() => HandleConnection(newHostSocket));
                        connectionThread.Start();
                        connectionThreads.Add(connectionThread);
                    }
                    else if (buffer[0] == 63)
                    {
                        timer.Restart();
                    }
                }
            }
            catch (ThreadAbortException) { }
        }

        static void HandleConnection(Socket newHostSocket)
        {
            ushort port;
            lock (serverPortMutex)
            {
                port = serverPort;
            }

            // Connect to local server
            IPAddress ipAddr = IPAddress.Parse("127.0.0.1");
            IPEndPoint localEndPoint = new IPEndPoint(ipAddr, port);

            Socket serverSocket = new Socket(ipAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            IAsyncResult result = serverSocket.ConnectAsync(localEndPoint);
            result.AsyncWaitHandle.WaitOne(5000);

            // Send connection result
            byte[] confirmationByte = new byte[1];
            confirmationByte[0] = Convert.ToByte(serverSocket.Connected);

            try
            {
                int sent = newHostSocket.Send(confirmationByte);
                if (sent == 0)
                {
                    Console.WriteLine($"Confirmation byte 0 bytes sent, closing connection.");
                    newHostSocket.Close();
                    serverSocket.Close();
                    return;
                }
            }
            catch (SocketException e)
            {
                Console.WriteLine($"[Exception on confirmation byte ({e.ErrorCode})] {e.Message}");
                newHostSocket.Close();
                serverSocket.Close();
                return;
            }

            if (serverSocket.Connected)
            {
                // Forward traffic
                Thread userToHostThread = new Thread(() => ForwardAToB(newHostSocket, serverSocket));
                userToHostThread.Start();
                ForwardAToB(serverSocket, newHostSocket);
                userToHostThread.Join();
            }
        }

        static void ForwardAToB(Socket a, Socket b)
        {
            byte[] buffer = new byte[1024];

            while (!THREAD_STOP_FLAG)
            {
                int received;
                try
                {
                    received = a.Receive(buffer);
                    if (received == 0)
                    {
                        Console.WriteLine("0 bytes reveived, closing connection.");
                        CloseConnection(a, b);
                        return;
                    }
                }
                catch (SocketException e)
                {
                    Console.WriteLine($"[Socket exception on receive ({e.ErrorCode})] " + e.Message);
                    CloseConnection(a, b);
                    return;
                }
                catch (Exception e)
                {
                    Console.WriteLine("[Receive exception] " + e.Message);
                    CloseConnection(a, b);
                    return;
                }

                int sent = 0;
                do
                {
                    try
                    {
                        int _sent = b.Send(buffer, sent, received - sent, SocketFlags.None);
                        if (_sent == 0)
                        {
                            Console.WriteLine("0 bytes sent, closing connection.");
                            CloseConnection(a, b);
                            return;
                        }
                        sent += _sent;
                    }
                    catch (SocketException e)
                    {
                        Console.WriteLine($"[Socket exception on send ({e.ErrorCode})] " + e.Message);
                        CloseConnection(a, b);
                        return;
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("[Receive exception] " + e.Message);
                        CloseConnection(a, b);
                        return;
                    }

                }
                while (sent < received);
            }

            CloseConnection(a, b);
        }

        static void CloseConnection(Socket a, Socket b)
        {
            a.Close();
            b.Close();
        }
    }
}
