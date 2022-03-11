
namespace LocalUI
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.forwardedPortDescLabel = new System.Windows.Forms.Label();
            this.currentPortLabel = new System.Windows.Forms.Label();
            this.changePortButton = new System.Windows.Forms.Button();
            this.connectionsAllowedDescLabel = new System.Windows.Forms.Label();
            this.connectionsAllowedLabel = new System.Windows.Forms.Label();
            this.toggleAllowConnectionsButton = new System.Windows.Forms.Button();
            this.serverIpDescLabel = new System.Windows.Forms.Label();
            this.connectionStatusDescLabel = new System.Windows.Forms.Label();
            this.serverIpLabel = new System.Windows.Forms.Label();
            this.changeServerIpButton = new System.Windows.Forms.Button();
            this.connectionStatusLabel = new System.Windows.Forms.Label();
            this.connectionButton = new System.Windows.Forms.Button();
            this.connectionsPanel = new System.Windows.Forms.Panel();
            this.disconnectAllButton = new System.Windows.Forms.Button();
            this.connectionSettingsButton = new System.Windows.Forms.Button();
            this.serverPortDescLabel = new System.Windows.Forms.Label();
            this.serverPortLabel = new System.Windows.Forms.Label();
            this.changeServerPortButton = new System.Windows.Forms.Button();
            this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.SuspendLayout();
            // 
            // forwardedPortDescLabel
            // 
            this.forwardedPortDescLabel.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.forwardedPortDescLabel.Location = new System.Drawing.Point(13, 13);
            this.forwardedPortDescLabel.Name = "forwardedPortDescLabel";
            this.forwardedPortDescLabel.Size = new System.Drawing.Size(121, 23);
            this.forwardedPortDescLabel.TabIndex = 0;
            this.forwardedPortDescLabel.Text = "Forwarded port:";
            this.forwardedPortDescLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // currentPortLabel
            // 
            this.currentPortLabel.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.currentPortLabel.Location = new System.Drawing.Point(140, 13);
            this.currentPortLabel.Name = "currentPortLabel";
            this.currentPortLabel.Size = new System.Drawing.Size(55, 23);
            this.currentPortLabel.TabIndex = 1;
            this.currentPortLabel.Text = "44444";
            this.currentPortLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // changePortButton
            // 
            this.changePortButton.Location = new System.Drawing.Point(206, 15);
            this.changePortButton.Name = "changePortButton";
            this.changePortButton.Size = new System.Drawing.Size(75, 23);
            this.changePortButton.TabIndex = 2;
            this.changePortButton.Text = "Change";
            this.changePortButton.UseVisualStyleBackColor = true;
            this.changePortButton.Click += new System.EventHandler(this.changePortButton_Click);
            // 
            // connectionsAllowedDescLabel
            // 
            this.connectionsAllowedDescLabel.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.connectionsAllowedDescLabel.Location = new System.Drawing.Point(13, 36);
            this.connectionsAllowedDescLabel.Name = "connectionsAllowedDescLabel";
            this.connectionsAllowedDescLabel.Size = new System.Drawing.Size(100, 23);
            this.connectionsAllowedDescLabel.TabIndex = 3;
            this.connectionsAllowedDescLabel.Text = "Connections:";
            this.connectionsAllowedDescLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // connectionsAllowedLabel
            // 
            this.connectionsAllowedLabel.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.connectionsAllowedLabel.ForeColor = System.Drawing.Color.Green;
            this.connectionsAllowedLabel.Location = new System.Drawing.Point(119, 36);
            this.connectionsAllowedLabel.Name = "connectionsAllowedLabel";
            this.connectionsAllowedLabel.Size = new System.Drawing.Size(76, 23);
            this.connectionsAllowedLabel.TabIndex = 4;
            this.connectionsAllowedLabel.Text = "Allowed";
            this.connectionsAllowedLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // toggleAllowConnectionsButton
            // 
            this.toggleAllowConnectionsButton.Location = new System.Drawing.Point(206, 38);
            this.toggleAllowConnectionsButton.Name = "toggleAllowConnectionsButton";
            this.toggleAllowConnectionsButton.Size = new System.Drawing.Size(75, 23);
            this.toggleAllowConnectionsButton.TabIndex = 5;
            this.toggleAllowConnectionsButton.Text = "Block";
            this.toggleAllowConnectionsButton.UseVisualStyleBackColor = true;
            this.toggleAllowConnectionsButton.Click += new System.EventHandler(this.toggleAllowConnectionsButton_Click);
            // 
            // serverIpDescLabel
            // 
            this.serverIpDescLabel.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.serverIpDescLabel.Location = new System.Drawing.Point(306, 13);
            this.serverIpDescLabel.Name = "serverIpDescLabel";
            this.serverIpDescLabel.Size = new System.Drawing.Size(76, 23);
            this.serverIpDescLabel.TabIndex = 6;
            this.serverIpDescLabel.Text = "Server IP:";
            this.serverIpDescLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // connectionStatusDescLabel
            // 
            this.connectionStatusDescLabel.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.connectionStatusDescLabel.Location = new System.Drawing.Point(306, 59);
            this.connectionStatusDescLabel.Name = "connectionStatusDescLabel";
            this.connectionStatusDescLabel.Size = new System.Drawing.Size(58, 23);
            this.connectionStatusDescLabel.TabIndex = 12;
            this.connectionStatusDescLabel.Text = "Status:";
            this.connectionStatusDescLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // serverIpLabel
            // 
            this.serverIpLabel.AutoEllipsis = true;
            this.serverIpLabel.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.serverIpLabel.Location = new System.Drawing.Point(388, 13);
            this.serverIpLabel.Name = "serverIpLabel";
            this.serverIpLabel.Size = new System.Drawing.Size(131, 23);
            this.serverIpLabel.TabIndex = 7;
            this.serverIpLabel.Text = "127.0.0.1";
            this.serverIpLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // changeServerIpButton
            // 
            this.changeServerIpButton.Location = new System.Drawing.Point(525, 15);
            this.changeServerIpButton.Name = "changeServerIpButton";
            this.changeServerIpButton.Size = new System.Drawing.Size(75, 23);
            this.changeServerIpButton.TabIndex = 8;
            this.changeServerIpButton.Text = "Change";
            this.changeServerIpButton.UseVisualStyleBackColor = true;
            this.changeServerIpButton.Click += new System.EventHandler(this.changeServerIpButton_Click);
            // 
            // connectionStatusLabel
            // 
            this.connectionStatusLabel.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.connectionStatusLabel.ForeColor = System.Drawing.Color.Green;
            this.connectionStatusLabel.Location = new System.Drawing.Point(370, 59);
            this.connectionStatusLabel.Name = "connectionStatusLabel";
            this.connectionStatusLabel.Size = new System.Drawing.Size(149, 23);
            this.connectionStatusLabel.TabIndex = 13;
            this.connectionStatusLabel.Text = "Connected";
            this.connectionStatusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // connectionButton
            // 
            this.connectionButton.Location = new System.Drawing.Point(525, 61);
            this.connectionButton.Name = "connectionButton";
            this.connectionButton.Size = new System.Drawing.Size(75, 23);
            this.connectionButton.TabIndex = 14;
            this.connectionButton.Text = "Disconnect";
            this.connectionButton.UseVisualStyleBackColor = true;
            this.connectionButton.Click += new System.EventHandler(this.connectionButton_Click);
            // 
            // connectionsPanel
            // 
            this.connectionsPanel.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.connectionsPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.connectionsPanel.Location = new System.Drawing.Point(13, 90);
            this.connectionsPanel.Name = "connectionsPanel";
            this.connectionsPanel.Size = new System.Drawing.Size(587, 295);
            this.connectionsPanel.TabIndex = 15;
            // 
            // disconnectAllButton
            // 
            this.disconnectAllButton.Location = new System.Drawing.Point(13, 391);
            this.disconnectAllButton.Name = "disconnectAllButton";
            this.disconnectAllButton.Size = new System.Drawing.Size(100, 23);
            this.disconnectAllButton.TabIndex = 16;
            this.disconnectAllButton.Text = "Disconnect all";
            this.disconnectAllButton.UseVisualStyleBackColor = true;
            // 
            // connectionSettingsButton
            // 
            this.connectionSettingsButton.Location = new System.Drawing.Point(475, 391);
            this.connectionSettingsButton.Name = "connectionSettingsButton";
            this.connectionSettingsButton.Size = new System.Drawing.Size(125, 23);
            this.connectionSettingsButton.TabIndex = 17;
            this.connectionSettingsButton.Text = "Connection settings";
            this.connectionSettingsButton.UseVisualStyleBackColor = true;
            this.connectionSettingsButton.Click += new System.EventHandler(this.connectionSettingsButton_Click);
            // 
            // serverPortDescLabel
            // 
            this.serverPortDescLabel.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.serverPortDescLabel.Location = new System.Drawing.Point(306, 36);
            this.serverPortDescLabel.Name = "serverPortDescLabel";
            this.serverPortDescLabel.Size = new System.Drawing.Size(92, 23);
            this.serverPortDescLabel.TabIndex = 9;
            this.serverPortDescLabel.Text = "Server port:";
            this.serverPortDescLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // serverPortLabel
            // 
            this.serverPortLabel.AutoEllipsis = true;
            this.serverPortLabel.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.serverPortLabel.Location = new System.Drawing.Point(404, 36);
            this.serverPortLabel.Name = "serverPortLabel";
            this.serverPortLabel.Size = new System.Drawing.Size(115, 23);
            this.serverPortLabel.TabIndex = 10;
            this.serverPortLabel.Text = "44444";
            this.serverPortLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // changeServerPortButton
            // 
            this.changeServerPortButton.Location = new System.Drawing.Point(525, 38);
            this.changeServerPortButton.Name = "changeServerPortButton";
            this.changeServerPortButton.Size = new System.Drawing.Size(75, 23);
            this.changeServerPortButton.TabIndex = 11;
            this.changeServerPortButton.Text = "Change";
            this.changeServerPortButton.UseVisualStyleBackColor = true;
            this.changeServerPortButton.Click += new System.EventHandler(this.changeServerPortButton_Click);
            // 
            // notifyIcon
            // 
            this.notifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon.Icon")));
            this.notifyIcon.Text = "Traffic forwarder";
            this.notifyIcon.MouseClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon_MouseDoubleClick);
            this.notifyIcon.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon_MouseDoubleClick);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(614, 428);
            this.Controls.Add(this.changeServerPortButton);
            this.Controls.Add(this.serverPortLabel);
            this.Controls.Add(this.serverPortDescLabel);
            this.Controls.Add(this.connectionSettingsButton);
            this.Controls.Add(this.disconnectAllButton);
            this.Controls.Add(this.connectionsPanel);
            this.Controls.Add(this.connectionButton);
            this.Controls.Add(this.connectionStatusLabel);
            this.Controls.Add(this.changeServerIpButton);
            this.Controls.Add(this.serverIpLabel);
            this.Controls.Add(this.connectionStatusDescLabel);
            this.Controls.Add(this.serverIpDescLabel);
            this.Controls.Add(this.toggleAllowConnectionsButton);
            this.Controls.Add(this.connectionsAllowedLabel);
            this.Controls.Add(this.connectionsAllowedDescLabel);
            this.Controls.Add(this.changePortButton);
            this.Controls.Add(this.currentPortLabel);
            this.Controls.Add(this.forwardedPortDescLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Traffic forwarder";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.HandleCreated += new System.EventHandler(this.MainForm_Load);
            this.Resize += new System.EventHandler(this.MainForm_Resize);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label forwardedPortDescLabel;
        private System.Windows.Forms.Label currentPortLabel;
        private System.Windows.Forms.Button cahngePortButton;
        private System.Windows.Forms.Button changePortButton;
        private System.Windows.Forms.Label connectionsAllowedDescLabel;
        private System.Windows.Forms.Label connectionsAllowedLabel;
        private System.Windows.Forms.Button toggleAllowConnectionsButton;
        private System.Windows.Forms.Label serverIpDescLabel;
        private System.Windows.Forms.Label connectionStatusDescLabel;
        private System.Windows.Forms.Label serverIpLabel;
        private System.Windows.Forms.Button changeServerIpButton;
        private System.Windows.Forms.Label connectionStatusLabel;
        private System.Windows.Forms.Button connectionButton;
        private System.Windows.Forms.Panel connectionsPanel;
        private System.Windows.Forms.Button disconnectAllButton;
        private System.Windows.Forms.Button connectionSettingsButton;
        private System.Windows.Forms.Panel n;
        private System.Windows.Forms.Label serverPortDescLabel;
        private System.Windows.Forms.Label serverPortLabel;
        private System.Windows.Forms.Button changeServerPortButton;
        private System.Windows.Forms.NotifyIcon notifyIcon;
    }
}

