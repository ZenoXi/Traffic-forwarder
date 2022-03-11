using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace LocalUI
{
    public partial class MainForm : Form
    {
        private ConnectionManager _connectionManager;
        private Thread _backgroundUpdateThread;
        private bool _stopBackgroundThread;

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            //AllocConsole();

            currentPortLabel.Text = Program.options.LastLocalPort;
            serverIpLabel.Text = Program.options.LastRemoteIp;
            serverPortLabel.Text = Program.options.LastRemotePort;

            _connectionManager = new ConnectionManager();
            _connectionManager.LocalPort = ushort.Parse(currentPortLabel.Text);
            _connectionManager.AllowConnections = true;
            _stopBackgroundThread = false;
            _backgroundUpdateThread = new Thread(() => UpdateUI());
            _backgroundUpdateThread.Start();
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool AllocConsole();

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            _stopBackgroundThread = true;
            _backgroundUpdateThread.Join();
            _connectionManager.Disconnect();
            _connectionManager.Stop();
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            if (FormWindowState.Minimized == WindowState)
            {
                notifyIcon.Visible = true;
                Hide();
            }
            else if (FormWindowState.Normal == WindowState)
            {
                notifyIcon.Visible = false;
            }
        }

        private void notifyIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Show();
            WindowState = FormWindowState.Normal;
        }

        private void changePortButton_Click(object sender, EventArgs e)
        {
            ChangePortForm form = new ChangePortForm(currentPortLabel.Text);
            form.ShowDialog();
            if (form.port.Length != 0)
            {
                currentPortLabel.Text = form.port;
                Program.options.LastLocalPort = form.port;
                Program.options.Save();
                _connectionManager.LocalPort = ushort.Parse(form.port);
            }
        }

        private void changeServerIpButton_Click(object sender, EventArgs e)
        {
            ChangeIPForm form = new ChangeIPForm(serverIpLabel.Text, true);
            form.ShowDialog();
            if (form.ip.Length != 0)
            {
                serverIpLabel.Text = form.ip;
                Program.options.LastRemoteIp = form.ip;
                Program.options.Save();
                _connectionManager.Disconnect();
            }
        }

        private void changeServerPortButton_Click(object sender, EventArgs e)
        {
            ChangePortForm form = new ChangePortForm(serverPortLabel.Text);
            form.ShowDialog();
            if (form.port.Length != 0)
            {
                serverPortLabel.Text = form.port;
                Program.options.LastRemotePort = form.port;
                Program.options.Save();
                _connectionManager.Disconnect();
            }
        }

        private void toggleAllowConnectionsButton_Click(object sender, EventArgs e)
        {
            _connectionManager.AllowConnections = !_connectionManager.AllowConnections;
            if (_connectionManager.AllowConnections)
            {
                connectionsAllowedLabel.Text = "Allowed";
                connectionsAllowedLabel.ForeColor = Color.Green;
                toggleAllowConnectionsButton.Text = "Block";
            }
            else
            {
                connectionsAllowedLabel.Text = "Blocked";
                connectionsAllowedLabel.ForeColor = Color.Red;
                toggleAllowConnectionsButton.Text = "Allow";
            }
        }

        private void connectionButton_Click(object sender, EventArgs e)
        {
            switch (_connectionManager.State)
            {
                case ConnectionState.CONNECTED:
                {
                    _connectionManager.Disconnect();
                    break;
                }
                case ConnectionState.CONNECTING:
                {
                    _connectionManager.Disconnect();
                    break;
                }
                case ConnectionState.CONNECTION_FAILED:
                {
                    _connectionManager.Connect(serverIpLabel.Text, ushort.Parse(serverPortLabel.Text));
                    break;
                }
                case ConnectionState.CONNECTION_LOST:
                {
                    _connectionManager.Connect(serverIpLabel.Text, ushort.Parse(serverPortLabel.Text));
                    break;
                }
                case ConnectionState.DISCONNECTED:
                {
                    _connectionManager.Connect(serverIpLabel.Text, ushort.Parse(serverPortLabel.Text));
                    break;
                }
                case ConnectionState.RECONNECTING:
                {
                    _connectionManager.Disconnect();
                    break;
                }
            }
        }

        private void connectionSettingsButton_Click(object sender, EventArgs e)
        {
            ConnectionSettingsForm form = new ConnectionSettingsForm();
            form.ShowDialog();
            if (form.changesMade)
            {
                EnforceSettings();
                _connectionManager.SignalConnectionsChanged();
            }
        }

        private void EnforceSettings()
        {
            // Max connections
            if (Program.options.DisconnectExcess)
            {
                var cons = _connectionManager.AcquireConnections();
                for (int i = Program.options.MaxConnections; i < cons.Count; i++)
                {
                    cons[i].Close();
                }
                _connectionManager.ReleaseConnections();
            }

            // Whitelist
            if (Program.options.WhitelistEnabled)
            {
                var cons = _connectionManager.AcquireConnections();
                foreach (var con in cons)
                {
                    if (!Program.options.Whitelist.Contains(con.UserIp))
                    {
                        con.Close();
                    }
                }
                _connectionManager.ReleaseConnections();
            }

            // Blacklist
            if (Program.options.BlacklistEnabled)
            {
                var cons = _connectionManager.AcquireConnections();
                foreach (var con in cons)
                {
                    if (Program.options.Blacklist.Contains(con.UserIp))
                    {
                        con.Close();
                    }
                }
                _connectionManager.ReleaseConnections();
            }
        }

        private void UpdateUI()
        {
            Stopwatch updateTimer = new Stopwatch();
            updateTimer.Start();
            ConnectionState lastState = ConnectionState.UNKNOWN;

            while (!_stopBackgroundThread)
            {
                // Update server connection status label
                if (lastState != _connectionManager.State)
                {
                    lastState = _connectionManager.State;
                    connectionStatusLabel.Invoke(new Action(delegate ()
                    {
                        switch (lastState)
                        {
                            case ConnectionState.CONNECTED:
                            {
                                connectionStatusLabel.Text = "Connected";
                                connectionStatusLabel.ForeColor = Color.Green;
                                connectionButton.Text = "Disconnect";
                                break;
                            }
                            case ConnectionState.CONNECTING:
                            {
                                connectionStatusLabel.Text = "Connecting";
                                connectionStatusLabel.ForeColor = Color.Goldenrod;
                                connectionButton.Text = "Cancel";
                                break;
                            }
                            case ConnectionState.CONNECTION_FAILED:
                            {
                                connectionStatusLabel.Text = "Connection failed";
                                connectionStatusLabel.ForeColor = Color.Red;
                                connectionButton.Text = "Connect";
                                break;
                            }
                            case ConnectionState.CONNECTION_LOST:
                            {
                                connectionStatusLabel.Text = "Connection lost";
                                connectionStatusLabel.ForeColor = Color.Red;
                                connectionButton.Text = "Reconnect";
                                break;
                            }
                            case ConnectionState.DISCONNECTED:
                            {
                                connectionStatusLabel.Text = "Disconnected";
                                connectionStatusLabel.ForeColor = Color.Red;
                                connectionButton.Text = "Connect";
                                break;
                            }
                            case ConnectionState.RECONNECTING:
                            {
                                connectionStatusLabel.Text = "Reconnecting";
                                connectionStatusLabel.ForeColor = Color.Goldenrod;
                                connectionButton.Text = "Cancel";
                                break;
                            }
                        }
                    }));
                }

                // Update connections panel
                if (_connectionManager.ConnectionsChanged)
                {
                    connectionsPanel.Invoke(new Action(delegate ()
                    {
                        connectionsPanel.Controls.Clear();
                    }));

                    var connections = _connectionManager.AcquireConnections();
                    for (int i = 0; i < connections.Count; i++)
                    {
                        Connection connection = connections[i];
                        string connectionIp = connection.UserIp;
                        int connectionId = connection.ID;

                        // CONNECTION PANEL
                        Panel connectionPanel = new Panel();
                        connectionPanel.Location = new Point(0, i * 23);
                        connectionPanel.Size = new Size(connectionsPanel.Size.Width, 23);
                        if (i % 2 == 1)
                        {
                            connectionPanel.BackColor = Color.FromArgb(230, 230, 230);
                        }
                        else
                        {
                            connectionPanel.BackColor = Color.FromArgb(240, 240, 240);
                        }

                        // IP LABEL
                        Label ipLabel = new Label();
                        ipLabel.Text = connectionIp;
                        ipLabel.AutoSize = false;
                        ipLabel.Location = new Point(0, 0);
                        ipLabel.Size = new Size(120, 23);
                        ipLabel.TextAlign = ContentAlignment.MiddleLeft;

                        // PORT LABEL
                        Label portLabel = new Label();
                        portLabel.Text = connection.LocalPort.ToString();
                        if (connection.Blocked)
                        {
                            portLabel.Text = "Blocked";
                            portLabel.ForeColor = Color.Red;
                        }
                        portLabel.AutoSize = false;
                        portLabel.Location = new Point(120, 0);
                        portLabel.Size = new Size(60, 23);
                        portLabel.TextAlign = ContentAlignment.MiddleLeft;

                        // IDLE LABEL
                        Label idleLabel = new Label();
                        idleLabel.Text = "(Idle)";
                        idleLabel.AutoSize = false;
                        idleLabel.Location = new Point(190, 0);
                        idleLabel.Size = new Size(60, 23);
                        idleLabel.TextAlign = ContentAlignment.MiddleLeft;

                        // CLOSE CONNECTION LABEL
                        Label closeConnectionLabel = new Label();
                        closeConnectionLabel.Text = "Close";
                        closeConnectionLabel.AutoSize = false;
                        closeConnectionLabel.Location = new Point(connectionPanel.Size.Width - 60, 0);
                        closeConnectionLabel.Size = new Size(50, 23);
                        closeConnectionLabel.TextAlign = ContentAlignment.MiddleCenter;
                        closeConnectionLabel.ForeColor = Color.Red;
                        closeConnectionLabel.Click += new EventHandler((o, s) =>
                        {
                            Connection con = _connectionManager.AcquireConnections().FirstOrDefault(item => item.ID == connectionId);
                            if (con != null)
                            {
                                con.Close();
                            }
                            _connectionManager.ReleaseConnections();
                        });

                        // BLACKLIST LABEL
                        bool blacklisted = Program.options.Blacklist.Contains(connection.UserIp);
                        Label blacklistLabel = new Label();
                        if (!blacklisted)
                            blacklistLabel.Text = "Blacklist";
                        else
                            blacklistLabel.Text = "Unblacklist";
                        blacklistLabel.AutoSize = false;
                        blacklistLabel.Location = new Point(connectionPanel.Size.Width - 180, 0);
                        blacklistLabel.Size = new Size(120, 23);
                        blacklistLabel.TextAlign = ContentAlignment.MiddleCenter;
                        blacklistLabel.ForeColor = Color.Blue;
                        blacklistLabel.Click += new EventHandler((o, s) =>
                        {
                            if (!blacklisted)
                                Program.options.Blacklist.Add(connectionIp);
                            else
                                Program.options.Blacklist.Remove(connectionIp);
                            _connectionManager.SignalConnectionsChanged();
                            EnforceSettings();
                        });

                        // WHITELIST LABEL
                        bool whitelisted = Program.options.Whitelist.Contains(connection.UserIp);
                        Label whitelistLabel = new Label();
                        if (!whitelisted)
                            whitelistLabel.Text = "Whitelist";
                        else
                            whitelistLabel.Text = "Unwhitelist";
                        whitelistLabel.AutoSize = false;
                        whitelistLabel.Location = new Point(connectionPanel.Size.Width - 300, 0);
                        whitelistLabel.Size = new Size(120, 23);
                        whitelistLabel.TextAlign = ContentAlignment.MiddleCenter;
                        whitelistLabel.ForeColor = Color.Blue;
                        whitelistLabel.Click += new EventHandler((o, s) =>
                        {
                            if (!whitelisted)
                                Program.options.Whitelist.Add(connectionIp);
                            else
                                Program.options.Whitelist.Remove(connectionIp);
                            _connectionManager.SignalConnectionsChanged();
                            EnforceSettings();
                        });

                        // NEST CONTROLS
                        connectionPanel.Controls.Add(ipLabel);
                        connectionPanel.Controls.Add(portLabel);
                        if (connection.Idle)
                        {
                            connectionPanel.Controls.Add(idleLabel);
                        }
                        connectionPanel.Controls.Add(closeConnectionLabel);
                        connectionPanel.Controls.Add(blacklistLabel);
                        connectionPanel.Controls.Add(whitelistLabel);
                        connectionsPanel.Invoke(new Action(delegate()
                        {
                            connectionsPanel.Controls.Add(connectionPanel);
                        }));
                    }
                    _connectionManager.ReleaseConnections();
                }

                Thread.Sleep(10);
            }
        }
    }
}
