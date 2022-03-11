using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace LocalUI
{
    class Connection
    {
        public int ID { get; private set; }
        public bool Connecting { get; private set; }
        public bool ConnectedToRemote { get; private set; }
        public bool ConnectedToLocal { get; private set; }
        public bool Blocked { get; private set; }
        public bool Idle { get; set; }
        public ushort LocalPort { get; private set; }
        public string UserIp { get; private set; }
        public float Score { get; set; }

        private Socket _remoteSocket;
        private Socket _localSocket;

        private class StorageBuffer
        {
            public byte[] data = new byte[1024];
            public int pos = 0;
            public int used = 0;
        }
        private StorageBuffer _bufferLR = new StorageBuffer();
        private StorageBuffer _bufferRL = new StorageBuffer();
        private byte[] _buffer = new byte[1024];

        private Thread _connectionThread;
        private bool _THREAD_STOP_FLAG;

        public Connection(int id, Socket serverSocket, ushort localPort, string userIp, bool block = false)
        {
            ID = id;
            _remoteSocket = serverSocket;
            LocalPort = localPort;
            UserIp = userIp;
            Blocked = block;
            ConnectedToLocal = false;
            ConnectedToRemote = false;

            if (Blocked)
            {
                CloseConnection(_remoteSocket, _localSocket);
                return;
            }

            Connecting = true;
            _THREAD_STOP_FLAG = false;
            _connectionThread = new Thread(() => HandleConnection());
            _connectionThread.Start();
        }

        public void Close()
        {
            CloseConnection(_localSocket, _remoteSocket);
            if (_connectionThread != null && _connectionThread.IsAlive)
            {
                _THREAD_STOP_FLAG = true;
                _connectionThread.Join();
            }
        }

        void HandleConnection()
        {
            // Connect to local server
            IPAddress ipAddr = IPAddress.Parse("127.0.0.1");
            IPEndPoint localEndPoint = new IPEndPoint(ipAddr, LocalPort);

            _localSocket = new Socket(ipAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            IAsyncResult result = _localSocket.ConnectAsync(localEndPoint);
            //result.AsyncWaitHandle.WaitOne(5000);

            Stopwatch timeoutTimer = new Stopwatch();
            timeoutTimer.Start();
            while (!_THREAD_STOP_FLAG)
            {
                if (result.IsCompleted || timeoutTimer.ElapsedMilliseconds > 5000)
                {
                    break;
                }
                Thread.Sleep(10);
            }

            // Send connection result
            byte[] confirmationByte = new byte[1];
            confirmationByte[0] = Convert.ToByte(_localSocket.Connected);
            try
            {
                int sent = _remoteSocket.Send(confirmationByte);
                if (sent == 0)
                {
                    CloseConnection(_remoteSocket, _localSocket);
                    return;
                }
            }
            catch (SocketException)
            {
                CloseConnection(_remoteSocket, _localSocket);
                return;
            }

            if (_localSocket.Connected)
            {
                ConnectedToLocal = true;
                ConnectedToRemote = true;
                _localSocket.Blocking = false;
                _remoteSocket.Blocking = false;
                Connecting = false;

                // Forward traffic
                //Thread remoteToLocalThread = new Thread(() => ForwardAToB(_remoteSocket, _localSocket));
                //remoteToLocalThread.Start();
                //ForwardAToB(_localSocket, _remoteSocket);
                //remoteToLocalThread.Join();
            }
        }

        void ForwardAToB(Socket a, Socket b)
        {
            byte[] buffer = new byte[1024];

            while (!_THREAD_STOP_FLAG)
            {
                int received;
                try
                {
                    received = a.Receive(buffer);
                    if (received == 0)
                    {
                        CloseConnection(a, b);
                        return;
                    }
                }
                catch (SocketException e)
                {
                    CloseConnection(a, b);
                    return;
                }
                catch (Exception e)
                {
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
                            CloseConnection(a, b);
                            return;
                        }
                        sent += _sent;
                    }
                    catch (SocketException e)
                    {
                        CloseConnection(a, b);
                        return;
                    }
                    catch (Exception e)
                    {
                        CloseConnection(a, b);
                        return;
                    }

                }
                while (sent < received);
            }

            CloseConnection(a, b);
        }

        public int Tick()
        {
            if (!ConnectedToLocal || !ConnectedToRemote)
                return 0;

            int totalBytes = 0;
            totalBytes += TickAToB(_localSocket, _remoteSocket, _bufferLR);
            totalBytes += TickAToB(_remoteSocket, _localSocket, _bufferRL);
            return totalBytes;
        }

        int TickAToB(Socket sockA, Socket sockB, StorageBuffer buffer)
        {
            // Read incoming traffic
            int received = 0;
            if (buffer.used < buffer.data.Length)
            {
                try
                {
                    received = sockA.Receive(_buffer, buffer.data.Length - buffer.used, SocketFlags.None);
                    if (received == 0)
                    {
                        CloseConnection(sockA, sockB);
                        return 0;
                    }
                }
                catch (SocketException e)
                {
                    if (e.SocketErrorCode != SocketError.WouldBlock)
                    {
                        CloseConnection(sockA, sockB);
                        return 0;
                    }
                }
                catch (Exception)
                {
                    CloseConnection(sockA, sockB);
                    return 0;
                }
            }

            // Add received bytes to the storage buffer
            if (received > 0)
            {
                int part1 = received;
                if (part1 > buffer.data.Length - buffer.pos)
                {
                    part1 = buffer.data.Length - buffer.pos;
                }
                int part2 = received - part1;
                if (part1 > 0)
                {
                    Buffer.BlockCopy(_buffer, 0, buffer.data, buffer.pos, part1);
                    buffer.pos = (buffer.pos + part1) % buffer.data.Length;
                }
                if (part2 > 0)
                {
                    Buffer.BlockCopy(_buffer, part1, buffer.data, 0, part2);
                    buffer.pos = part2;
                }
                buffer.used += received;
            }

            // Check if there is anything to send
            if (buffer.used == 0)
                return 0;

            // Copy remaining storage buffer to out buffer
            {
                int startPos = (buffer.pos + buffer.data.Length - buffer.used) % buffer.data.Length;
                int part1 = buffer.used;
                if (part1 > buffer.data.Length - startPos)
                {
                    part1 = buffer.data.Length - startPos;
                }
                int part2 = buffer.used - part1;
                if (part1 > 0)
                {
                    Buffer.BlockCopy(buffer.data, startPos, _buffer, 0, part1);
                }
                if (part2 > 0)
                {
                    Buffer.BlockCopy(buffer.data, 0, _buffer, part1, part2);
                }
            }

            {
                //int sentTotal = 0;
                //while (buffer.used > 0)
                //{
                //    // Send out buffer
                //    int sent = 0;
                //    try
                //    {
                //        sent = sockB.Send(_buffer, sentTotal, buffer.used, SocketFlags.None);
                //        if (sent == 0)
                //        {
                //            CloseConnection(sockA, sockB);
                //            return 0;
                //        }
                //    }
                //    catch (SocketException e)
                //    {
                //        if (e.SocketErrorCode != SocketError.WouldBlock)
                //        {
                //            CloseConnection(sockA, sockB);
                //            return 0;
                //        }
                //    }
                //    catch (Exception)
                //    {
                //        CloseConnection(sockA, sockB);
                //        return 0;
                //    }
                //    buffer.used -= sent;
                //    sentTotal += sent;
                //}
                //return sentTotal;
            }

            // Send out buffer
            int sent = 0;
            try
            {
                sent = sockB.Send(_buffer, buffer.used, SocketFlags.None);
                if (sent == 0)
                {
                    CloseConnection(sockA, sockB);
                    return 0;
                }
            }
            catch (SocketException e)
            {
                if (e.SocketErrorCode != SocketError.WouldBlock)
                {
                    CloseConnection(sockA, sockB);
                    return 0;
                }
            }
            catch (Exception)
            {
                CloseConnection(sockA, sockB);
                return 0;
            }
            buffer.used -= sent;
            return sent;
        }

        void CloseConnection(Socket a, Socket b)
        {
            ConnectedToRemote = false;
            ConnectedToLocal = false;
            if (a != null) a.Close();
            if (b != null) b.Close();
        }
    }
}
