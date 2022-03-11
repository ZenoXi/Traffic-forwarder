using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Remote
{
    class Program
    {
        static ushort hostListenerPort;
        static ushort userListenerPort;
        static TcpListener hostListener;
        static TcpListener userListener;

        static Socket hostSocket;

        static List<Thread> connectionThreads = new List<Thread>();

        static bool STOP_FLAG = false;

        static void Main(string[] args)
        {
            // Input ports
            Console.Write("Host port: ");
            string hostPort = Console.ReadLine();
            if (!ushort.TryParse(hostPort, out hostListenerPort))
            {
                Console.WriteLine("Bad port");
                return;
            }
            Console.Write("User port: ");
            string userPort = Console.ReadLine();
            if (!ushort.TryParse(userPort, out userListenerPort))
            {
                Console.WriteLine("Bad port");
                return;
            }

            hostListener = new TcpListener(IPAddress.Any, hostListenerPort);
            hostListener.Server.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            hostListener.Start(1);

            if (!WaitForMainConnection())
            {
                return;
            }
            WaitForUserConnections();

            // Stop all threads
            STOP_FLAG = true;
            foreach (var thread in connectionThreads)
            {
                thread.Join();
            }
        }

        static bool WaitForMainConnection()
        {
            Console.WriteLine("Waiting for host connection, press 's' to abort");
            while (!hostListener.Pending())
            {
                Thread.Sleep(10);
                if (Console.KeyAvailable)
                {
                    if (Console.ReadKey().KeyChar == 's')
                    {
                        Console.WriteLine("Aborted.");
                        return false;
                    }
                }
            }
            hostSocket = hostListener.AcceptSocket();
            Console.WriteLine("Host connected!");
            return true;
        }

        static void WaitForUserConnections()
        {
            userListener = new TcpListener(IPAddress.Any, userListenerPort);
            userListener.Server.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            userListener.Start();

            Console.WriteLine("Waiting for connections, press 's' to abort");
            
            byte[] connectionMsg = new byte[5];
            byte[] notificationMsg = new byte[5];
            connectionMsg[0] = 127;
            notificationMsg[0] = 63;

            Stopwatch timer = new Stopwatch();
            timer.Start();

            while (true)
            {
                // Check for abort key
                if (Console.KeyAvailable)
                {
                    if (Console.ReadKey().KeyChar == 's')
                    {
                        Console.WriteLine("Aborted.");
                        return;
                    }
                }

                // Check if host is still connected
                if (!hostSocket.Connected)
                {
                    hostSocket.Close();
                    Console.WriteLine("Host disconnected..");
                    if (!WaitForMainConnection())
                    {
                        return;
                    }
                }

                // Send notification packet to host
                if (timer.ElapsedMilliseconds > 5000)
                {
                    timer.Restart();
                    try
                    {
                        hostSocket.Send(notificationMsg);
                    }
                    catch (Exception) { continue; }
                }

                // Check for new user connection
                if (!userListener.Pending())
                {
                    Thread.Sleep(10);
                    continue;
                }
                Socket newUserSocket = userListener.AcceptSocket();
                Console.WriteLine("New user connection..");

                // Notify host
                try
                {
                    // Extract IP address
                    IPAddress addr = IPAddress.Parse(((IPEndPoint)newUserSocket.RemoteEndPoint).Address.ToString());
                    connectionMsg[1] = addr.GetAddressBytes()[0];
                    connectionMsg[2] = addr.GetAddressBytes()[1];
                    connectionMsg[3] = addr.GetAddressBytes()[2];
                    connectionMsg[4] = addr.GetAddressBytes()[3];

                    // Send notification
                    int sent = 0;
                    while (sent < 5)
                    {
                        sent += hostSocket.Send(connectionMsg, sent, 5 - sent, SocketFlags.None);
                    }
                }
                catch (Exception)
                {
                    newUserSocket.Close();
                    continue;
                }

                // Wait for new host connection
                int cycleCount = 0;
                while (!hostListener.Pending())
                {
                    Thread.Sleep(10);
                    if (Console.KeyAvailable)
                    {
                        if (Console.ReadKey().KeyChar == 's')
                        {
                            Console.WriteLine("Aborted.");
                            return;
                        }
                    }
                    cycleCount++;
                    if (cycleCount > 500)
                    {
                        break;
                    }
                }

                // Accept host connection
                Socket newHostSocket = null;
                if (hostListener.Pending())
                {
                    newHostSocket = hostListener.AcceptSocket();
                }
                else
                {
                    Console.WriteLine("Timeout.");
                    newUserSocket.Close();
                    continue;
                }
                Console.WriteLine("Host connected!");

                // Move connection to a new thread
                Thread newConnectionThread = new Thread(() => HandleConnection(newHostSocket, newUserSocket));
                newConnectionThread.Start();
                connectionThreads.Add(newConnectionThread);
            }
        }

        static void HandleConnection(Socket newHostSocket, Socket newUserSocket)
        {
            byte[] buffer = new byte[1];

            // Wait for connection confirmation
            newHostSocket.ReceiveTimeout = 10000;
            try
            {
                int received = newHostSocket.Receive(buffer);
                if (received == 0)
                {
                    CloseConnection(newHostSocket, newUserSocket);
                    return;
                }
            }
            catch
            {
                CloseConnection(newHostSocket, newUserSocket);
                return;
            }

            // Check for connection fail
            if (buffer[0] == 0)
            {
                CloseConnection(newHostSocket, newUserSocket);
                return;
            }

            // Reset timeout
            newHostSocket.ReceiveTimeout = 0;

            // Forward traffic
            Thread userToHostThread = new Thread(() => ForwardAToB(newUserSocket, newHostSocket));
            userToHostThread.Start();
            ForwardAToB(newHostSocket, newUserSocket);
            userToHostThread.Join();
        }

        static void ForwardHostToUser(Socket newHostSocket, Socket newUserSocket)
        {
            //constexpr int BUF_SIZE = 1024;
            //char buf[BUF_SIZE];

            //while (true)
            //{
            //    int received = recv(serverSocket, buf, BUF_SIZE, 0);
            //    if (received == SOCKET_ERROR)
            //    {
            //        int error = WSAGetLastError();
            //        if (error != WSAEWOULDBLOCK)
            //        {
            //            std::cout << "socket error " << error << std::endl;
            //            closesocket(userSocket);
            //            closesocket(serverSocket);
            //            return;
            //        }
            //    }
            //    if (received == 0)
            //    {
            //        closesocket(userSocket);
            //        closesocket(serverSocket);
            //        return;
            //    }
            //    if (received > 0)
            //    {
            //        int sent = 0;
            //        do
            //        {
            //            sent += send(userSocket, buf, received, 0);
            //        }
            //        while (sent < received);
            //    }
            //}

            byte[] buffer = new byte[1024];

            while (true)
            {
                int received = 0;
                try
                {
                    received = newHostSocket.Receive(buffer);
                    if (received == 0)
                    {
                        CloseConnection(newHostSocket, newUserSocket);
                        return;
                    }
                }
                catch
                {
                    CloseConnection(newHostSocket, newUserSocket);
                    return;
                }

                int sent = 0;
                do
                {
                    try
                    {
                        int _sent = newUserSocket.Send(buffer, sent, received - sent, SocketFlags.None);
                        if (_sent == 0)
                        {
                            CloseConnection(newHostSocket, newUserSocket);
                            return;
                        }
                        sent += _sent;
                    }
                    catch
                    {
                        CloseConnection(newHostSocket, newUserSocket);
                        return;
                    }
                    
                }
                while (sent < received);
            }
        }

        static void ForwardUserToHost(Socket newHostSocket, Socket newUserSocket)
        {
            byte[] buffer = new byte[1024];

            while (true)
            {
                int received = 0;
                try
                {
                    received = newUserSocket.Receive(buffer);
                    if (received == 0)
                    {
                        CloseConnection(newHostSocket, newUserSocket);
                        return;
                    }
                }
                catch
                {
                    CloseConnection(newHostSocket, newUserSocket);
                    return;
                }

                int sent = 0;
                do
                {
                    try
                    {
                        int _sent = newHostSocket.Send(buffer, sent, received - sent, SocketFlags.None);
                        if (_sent == 0)
                        {
                            CloseConnection(newHostSocket, newUserSocket);
                            return;
                        }
                        sent += _sent;
                    }
                    catch
                    {
                        CloseConnection(newHostSocket, newUserSocket);
                        return;
                    }

                }
                while (sent < received);
            }
        }

        static void ForwardAToB(Socket a, Socket b)
        {
            byte[] buffer = new byte[1024];

            while (!STOP_FLAG)
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
