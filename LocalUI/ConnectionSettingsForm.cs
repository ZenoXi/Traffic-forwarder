using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace LocalUI
{
    public partial class ConnectionSettingsForm : Form
    {
        public bool changesMade = false;

        public ConnectionSettingsForm()
        {
            InitializeComponent();
            LoadFromOptions();
        }

        private void LoadFromOptions()
        {
            maxConnectionsSelection.Value = Program.options.MaxConnections;
            disconnectExcessCheckBox.Checked = Program.options.DisconnectExcess;

            // Whitelist
            UpdateWhitelistStatus();
            UpdateWhitelist();

            // Blacklist
            UpdateBlacklistStatus();
            UpdateBlacklist();
        }

        private void UpdateWhitelistStatus()
        {
            if (Program.options.WhitelistEnabled)
            {
                whitelistLabel.Text = "Enabled";
                whitelistLabel.ForeColor = Color.Green;
                whitelistToggleButton.Text = "Disable";
            }
            else
            {
                whitelistLabel.Text = "Disabled";
                whitelistLabel.ForeColor = Color.Red;
                whitelistToggleButton.Text = "Enable";
            }
        }

        private void UpdateWhitelist()
        {
            whitelistPanel.Controls.Clear();
            for (int i = 0; i < Program.options.Whitelist.Count; i++)
            {
                string ip = Program.options.Whitelist[i];

                Panel ipPanel = new Panel();
                ipPanel.Location = new Point(0, i * 23);
                ipPanel.Size = new Size(whitelistPanel.Size.Width, 23);
                if (i % 2 == 1)
                {
                    ipPanel.BackColor = Color.FromArgb(230, 230, 230);
                }
                else
                {
                    ipPanel.BackColor = Color.FromArgb(240, 240, 240);
                }

                Label ipLabel = new Label();
                ipLabel.Text = ip;
                ipLabel.AutoSize = false;
                ipLabel.Location = new Point(0, 0);
                ipLabel.Size = new Size(ipPanel.Size.Width - 60, 23);
                ipLabel.TextAlign = ContentAlignment.MiddleLeft;

                Label ipRemoveLabel = new Label();
                ipRemoveLabel.Text = "Remove";
                ipRemoveLabel.AutoSize = false;
                ipRemoveLabel.Location = new Point(ipPanel.Size.Width - 60, 0);
                ipRemoveLabel.Size = new Size(50, 23);
                ipRemoveLabel.TextAlign = ContentAlignment.MiddleCenter;
                ipRemoveLabel.ForeColor = Color.Blue;
                ipRemoveLabel.Click += new EventHandler((o, s) =>
                {
                    Program.options.Whitelist.RemoveAll(item => item == ip);
                    UpdateWhitelist();
                    changesMade = true;
                });

                ipPanel.Controls.Add(ipLabel);
                ipPanel.Controls.Add(ipRemoveLabel);
                whitelistPanel.Controls.Add(ipPanel);
            }
        }

        private void UpdateBlacklistStatus()
        {
            if (Program.options.BlacklistEnabled)
            {
                blacklistLabel.Text = "Enabled";
                blacklistLabel.ForeColor = Color.Green;
                blacklistToggleButton.Text = "Disable";
            }
            else
            {
                blacklistLabel.Text = "Disabled";
                blacklistLabel.ForeColor = Color.Red;
                blacklistToggleButton.Text = "Enable";
            }
        }

        private void UpdateBlacklist()
        {
            blacklistPanel.Controls.Clear();
            for (int i = 0; i < Program.options.Blacklist.Count; i++)
            {
                string ip = Program.options.Blacklist[i];

                Panel ipPanel = new Panel();
                ipPanel.Location = new Point(0, i * 23);
                ipPanel.Size = new Size(blacklistPanel.Size.Width, 23);
                if (i % 2 == 1)
                {
                    ipPanel.BackColor = Color.FromArgb(230, 230, 230);
                }
                else
                {
                    ipPanel.BackColor = Color.FromArgb(240, 240, 240);
                }

                Label ipLabel = new Label();
                ipLabel.Text = ip;
                ipLabel.AutoSize = false;
                ipLabel.Location = new Point(0, 0);
                ipLabel.Size = new Size(ipPanel.Size.Width - 60, 23);
                ipLabel.TextAlign = ContentAlignment.MiddleLeft;

                Label ipRemoveLabel = new Label();
                ipRemoveLabel.Text = "Remove";
                ipRemoveLabel.AutoSize = false;
                ipRemoveLabel.Location = new Point(ipPanel.Size.Width - 60, 0);
                ipRemoveLabel.Size = new Size(50, 23);
                ipRemoveLabel.TextAlign = ContentAlignment.MiddleCenter;
                ipRemoveLabel.ForeColor = Color.Blue;
                ipRemoveLabel.Click += new EventHandler((o, s) =>
                {
                    Program.options.Blacklist.RemoveAll(item => item == ip);
                    UpdateBlacklist();
                    changesMade = true;
                });

                ipPanel.Controls.Add(ipLabel);
                ipPanel.Controls.Add(ipRemoveLabel);
                blacklistPanel.Controls.Add(ipPanel);
            }
        }

        private void ConnectionSettingsForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (changesMade)
            {
                Program.options.Save();
            }
        }

        private void whitelistToggleButton_Click(object sender, EventArgs e)
        {
            if (Program.options.WhitelistEnabled)
            {
                Program.options.WhitelistEnabled = false;
            }
            else
            {
                Program.options.WhitelistEnabled = true;
            }
            changesMade = true;
            UpdateWhitelistStatus();
        }

        private void whitelistAddButton_Click(object sender, EventArgs e)
        {
            string ip = whitelistAddTextBox.Text;
            if (Program.options.IpValid(ip))
            {
                if (!Program.options.Whitelist.Contains(ip))
                {
                    Program.options.Whitelist.Add(ip);
                    changesMade = true;
                    UpdateWhitelist();
                }
                whitelistAddTextBox.BackColor = SystemColors.Window;
                whitelistAddTextBox.Text = "";
            }
            else
            {
                whitelistAddTextBox.BackColor = Color.FromArgb(255, 128, 128);
            }
        }

        private void blacklistToggleButton_Click(object sender, EventArgs e)
        {
            if (Program.options.BlacklistEnabled)
            {
                Program.options.BlacklistEnabled = false;
            }
            else
            {
                Program.options.BlacklistEnabled = true;
            }
            changesMade = true;
            UpdateBlacklistStatus();
        }

        private void blacklistAddButton_Click(object sender, EventArgs e)
        {
            string ip = blacklistAddTextBox.Text;
            if (Program.options.IpValid(ip))
            {
                if (!Program.options.Blacklist.Contains(ip))
                {
                    Program.options.Blacklist.Add(ip);
                    changesMade = true;
                    UpdateBlacklist();
                }
                blacklistAddTextBox.BackColor = SystemColors.Window;
                blacklistAddTextBox.Text = "";
            }
            else
            {
                blacklistAddTextBox.BackColor = Color.FromArgb(255, 128, 128);
            }
        }

        private void maxConnectionsSelection_ValueChanged(object sender, EventArgs e)
        {
            Program.options.MaxConnections = (int)maxConnectionsSelection.Value;
            changesMade = true;
        }

        private void disconnectExcessCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            Program.options.DisconnectExcess = disconnectExcessCheckBox.Checked;
            changesMade = true;
        }
    }
}
