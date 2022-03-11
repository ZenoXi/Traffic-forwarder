using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace LocalUI
{
    enum ConnectionState
    {
        CONNECTED,
        DISCONNECTED,
        CONNECTING,
        RECONNECTING,
        CONNECTION_FAILED,
        CONNECTION_LOST,
        UNKNOWN
    }

    class ConnectionManager
    {
        public ushort LocalPort { get; set; }
        public string ServerIp { get; private set; }
        public ushort ServerPort { get; private set; }

        public ConnectionState State { get; private set; }

        private class ConnectionData
        {
            public Connection con;
            public int ticksIdle = 0;
            public int ticksNonIdle = 0;
            public float score = 1.0f;
            public float tickScore = 0.0f;
            public DateTime lastSend = DateTime.Now;
        }
        private List<ConnectionData> _connections;
        private Mutex _m_connections;
        private bool _connectionsChanged = false;
        public bool ConnectionsChanged {
            get
            {
                bool val = _connectionsChanged;
                _connectionsChanged = false;
                return val;
            }
        }
        public void SignalConnectionsChanged() { _connectionsChanged = true; }
        public bool AllowConnections { get; set; }
        private int _ID_COUNTER = 0;

        private Socket _serverSocket;
        private Thread _serverThread;
        private Thread _connectionThread;
        private bool _CONNECTION_STOP_FLAG;
        private Thread _connectionTickThread;
        private bool _CONNECTION_TICK_STOP_FLAG;

        public ConnectionManager()
        {
            LocalPort = 55555;
            State = ConnectionState.DISCONNECTED;
            _connections = new List<ConnectionData>();
            _m_connections = new Mutex();

            _CONNECTION_STOP_FLAG = false;
            _serverThread = new Thread(() => ProcessConnections());
            _serverThread.Start();
            _CONNECTION_TICK_STOP_FLAG = false;
            _connectionTickThread = new Thread(() => TickConnections());
            _connectionTickThread.Start();
        }

        ~ConnectionManager()
        {
            Stop();
            Disconnect();
        }

        public List<Connection> AcquireConnections()
        {
            _m_connections.WaitOne();
            return _connections.Select(item => item.con).ToList();
        }

        public void ReleaseConnections()
        {
            _m_connections.ReleaseMutex();
        }

        public void Connect(string serverIp, ushort serverPort)
        {
            if (State == ConnectionState.CONNECTING || State == ConnectionState.RECONNECTING)
            {
                return;
            }
            State = ConnectionState.CONNECTING;
            ServerIp = serverIp;
            ServerPort = serverPort;

            _connectionThread = new Thread(() => ConnectToServer());
            _connectionThread.Start();
        }

        public void Disconnect()
        {
            lock (_m_connections)
            {
                foreach (var connection in _connections)
                {
                    connection.con.Close();
                }
            }

            if (_serverSocket != null)
            {
                _serverSocket.Close();
            }
            State = ConnectionState.DISCONNECTED;
        }

        public void Stop()
        {
            _CONNECTION_STOP_FLAG = true;
            _CONNECTION_TICK_STOP_FLAG = true;
            _serverThread.Join();
            _connectionTickThread.Join();
        }

        public bool ConnectToServer(int attempts = 1)
        {
            IPAddress ipAddr = IPAddress.Parse(ServerIp);
            IPEndPoint serverEndPoint = new IPEndPoint(ipAddr, ServerPort);

            if (_serverSocket != null)
            {
                if (_serverSocket.Connected)
                {
                    try
                    {
                        _serverSocket.Disconnect(false);
                    }
                    catch (Exception) { }
                }
            }

            while (true)
            {
                _serverSocket = new Socket(ipAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                IAsyncResult result = _serverSocket.ConnectAsync(serverEndPoint);
                bool success = result.AsyncWaitHandle.WaitOne(5000);

                if (!_serverSocket.Connected)
                {
                    --attempts;
                    if (attempts == 0)
                    {
                        State = ConnectionState.CONNECTION_FAILED;
                        return false;
                    }
                    continue;
                }

                _serverSocket.Blocking = false;
                State = ConnectionState.CONNECTED;
                return true;
            }
        }

        private void ProcessConnections()
        {
            try
            {
                Stopwatch timer = new Stopwatch();
                timer.Start();

                byte[] buffer = new byte[5];
                int received = 0;
                while (!_CONNECTION_STOP_FLAG)
                {
                    if (State != ConnectionState.CONNECTED)
                    {
                        Thread.Sleep(10);
                        timer.Restart();
                        continue;
                    }

                    // If timer exceeds 10 seconds, restart the connection
                    if (timer.ElapsedMilliseconds > 10000)
                    {
                        State = ConnectionState.RECONNECTING;
                        if (!ConnectToServer())
                        {
                            State = ConnectionState.CONNECTION_LOST;
                            continue;
                        }
                        timer.Restart();
                    }

                    try
                    {
                        received = 0;
                        while (received < 5)
                        {
                            received += _serverSocket.Receive(buffer, received, 5 - received, SocketFlags.None);
                        }
                    }
                    catch (SocketException ex)
                    {
                        if (ex.SocketErrorCode == SocketError.WouldBlock)
                        {
                            Thread.Sleep(10);
                            continue;
                        }

                        State = ConnectionState.RECONNECTING;
                        if (!ConnectToServer())
                        {
                            State = ConnectionState.CONNECTION_LOST;
                        }
                        timer.Restart();
                        continue;
                    }
                    if (received == 0)
                    {
                        State = ConnectionState.DISCONNECTED;
                        continue;
                    }

                    if (buffer[0] == 127)
                    {
                        // Connect to host
                        IPAddress ipAddr = IPAddress.Parse(ServerIp);
                        IPEndPoint localEndPoint = new IPEndPoint(ipAddr, ServerPort);

                        Socket newServerSocket = new Socket(ipAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                        IAsyncResult result = newServerSocket.ConnectAsync(localEndPoint);
                        //bool success = result.AsyncWaitHandle.WaitOne(5000);

                        Stopwatch timeoutTimer = new Stopwatch();
                        timeoutTimer.Start();
                        while (!_CONNECTION_STOP_FLAG)
                        {
                            if (result.IsCompleted || timeoutTimer.ElapsedMilliseconds > 5000)
                            {
                                break;
                            }
                            Thread.Sleep(10);
                        }

                        if (!newServerSocket.Connected)
                        {
                            continue;
                        }

                        // Create connection object
                        string userIp = $"{buffer[1]}.{buffer[2]}.{buffer[3]}.{buffer[4]}";
                        bool blocked = !AllowConnections;
                        blocked |= Program.options.WhitelistEnabled && !Program.options.Whitelist.Contains(userIp);
                        blocked |= Program.options.BlacklistEnabled && Program.options.Blacklist.Contains(userIp);
                        Connection connection = new Connection(_ID_COUNTER++, newServerSocket, LocalPort, userIp, blocked);
                        ConnectionData conData = new ConnectionData();
                        conData.con = connection;
                        lock (_m_connections)
                        {
                            _connections.Add(conData);
                        }
                        _connectionsChanged = true;
                    }
                    else if (buffer[0] == 63)
                    {
                        timer.Restart();
                    }
                }
            }
            catch (ThreadAbortException) { }
        }

        private void TickConnections()
        {
            int noTrafficTicks = 0;
            while (!_CONNECTION_TICK_STOP_FLAG)
            {
                //Thread.Sleep(1000);
                //continue;
                bool trafficExists = false;
                lock (_m_connections)
                {
                    for (int i = 0; i < _connections.Count; i++)
                    {
                        var connection = _connections[i];

                        // Check connection status
                        if (connection.con.Connecting)
                            continue;
                        if (!connection.con.ConnectedToLocal || !connection.con.ConnectedToRemote)
                        {
                            connection.con.Close();
                            _connections.RemoveAt(i);
                            i--;
                            _connectionsChanged = true;
                            continue;
                        }

                        // Increment tick score
                        connection.tickScore += connection.score;
                        if (connection.tickScore < 1.0f)
                            continue;

                        // Tick
                        connection.tickScore = 0.0f;
                        int bytesMoved = connection.con.Tick();
                        if (bytesMoved > 0)
                        {
                            connection.ticksNonIdle += bytesMoved / 256;
                            connection.ticksIdle = 0;
                            trafficExists = true;
                            connection.lastSend = DateTime.Now;
                            if (connection.con.Idle)
                            {
                                _connectionsChanged = true;
                            }
                            connection.con.Idle = false;
                        }
                        else
                        {
                            connection.ticksIdle++;
                            connection.ticksNonIdle = 0;
                            int secondsIdle = (int)(DateTime.Now - connection.lastSend).TotalSeconds;
                            if (secondsIdle > 30)
                            {
                                if (!connection.con.Idle)
                                {
                                    _connectionsChanged = true;
                                }
                                connection.con.Idle = true;
                            }
                        }

                        // Calculate new score
                        if (connection.ticksIdle > 100)
                        {
                            connection.score *= 0.99f;
                            if (connection.score < 0.1f)
                                connection.score = 0.1f;
                        }
                        if (connection.ticksNonIdle > 0)
                        {
                            connection.score *= 1.01f + 0.01f * connection.ticksNonIdle;
                            if (connection.score > 1.0f)
                                connection.score = 1.0f;
                        }
                        connection.con.Score = connection.score;
                    }
                }
                
                if (!trafficExists)
                {
                    noTrafficTicks++;
                    if (noTrafficTicks > 1000)
                    {
                        noTrafficTicks = 1000;
                        Thread.Sleep(10);
                    }
                }
                else
                {
                    noTrafficTicks = 0;
                }
            }
        }
    }
}
