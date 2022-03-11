
namespace LocalUI
{
    partial class ConnectionSettingsForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.maxConnectionsDescLabel = new System.Windows.Forms.Label();
            this.maxConnectionsSelection = new System.Windows.Forms.NumericUpDown();
            this.separatorLabel1 = new System.Windows.Forms.Label();
            this.disconnectExcessCheckBox = new System.Windows.Forms.CheckBox();
            this.whitelistToggleButton = new System.Windows.Forms.Button();
            this.whitelistLabel = new System.Windows.Forms.Label();
            this.whitelistDescLabel = new System.Windows.Forms.Label();
            this.whitelistPanel = new System.Windows.Forms.Panel();
            this.whitelistAddTextBox = new System.Windows.Forms.TextBox();
            this.whitelistAddButton = new System.Windows.Forms.Button();
            this.blacklistAddButton = new System.Windows.Forms.Button();
            this.blacklistAddTextBox = new System.Windows.Forms.TextBox();
            this.blacklistPanel = new System.Windows.Forms.Panel();
            this.blacklistToggleButton = new System.Windows.Forms.Button();
            this.blacklistLabel = new System.Windows.Forms.Label();
            this.blacklistDescLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.maxConnectionsSelection)).BeginInit();
            this.SuspendLayout();
            // 
            // maxConnectionsDescLabel
            // 
            this.maxConnectionsDescLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.maxConnectionsDescLabel.Location = new System.Drawing.Point(12, 9);
            this.maxConnectionsDescLabel.Name = "maxConnectionsDescLabel";
            this.maxConnectionsDescLabel.Size = new System.Drawing.Size(106, 23);
            this.maxConnectionsDescLabel.TabIndex = 1;
            this.maxConnectionsDescLabel.Text = "Max connections:";
            this.maxConnectionsDescLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // maxConnectionsSelection
            // 
            this.maxConnectionsSelection.Location = new System.Drawing.Point(124, 11);
            this.maxConnectionsSelection.Maximum = new decimal(new int[] {
            99999,
            0,
            0,
            0});
            this.maxConnectionsSelection.Name = "maxConnectionsSelection";
            this.maxConnectionsSelection.Size = new System.Drawing.Size(120, 23);
            this.maxConnectionsSelection.TabIndex = 2;
            this.maxConnectionsSelection.ValueChanged += new System.EventHandler(this.maxConnectionsSelection_ValueChanged);
            // 
            // separatorLabel1
            // 
            this.separatorLabel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.separatorLabel1.Location = new System.Drawing.Point(12, 45);
            this.separatorLabel1.Name = "separatorLabel1";
            this.separatorLabel1.Size = new System.Drawing.Size(489, 1);
            this.separatorLabel1.TabIndex = 3;
            this.separatorLabel1.Text = "label1";
            // 
            // disconnectExcessCheckBox
            // 
            this.disconnectExcessCheckBox.Checked = true;
            this.disconnectExcessCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.disconnectExcessCheckBox.Location = new System.Drawing.Point(266, 10);
            this.disconnectExcessCheckBox.Name = "disconnectExcessCheckBox";
            this.disconnectExcessCheckBox.Size = new System.Drawing.Size(127, 24);
            this.disconnectExcessCheckBox.TabIndex = 5;
            this.disconnectExcessCheckBox.Text = "Disconnect excess";
            this.disconnectExcessCheckBox.UseVisualStyleBackColor = true;
            this.disconnectExcessCheckBox.CheckedChanged += new System.EventHandler(this.disconnectExcessCheckBox_CheckedChanged);
            // 
            // whitelistToggleButton
            // 
            this.whitelistToggleButton.Location = new System.Drawing.Point(177, 57);
            this.whitelistToggleButton.Name = "whitelistToggleButton";
            this.whitelistToggleButton.Size = new System.Drawing.Size(75, 23);
            this.whitelistToggleButton.TabIndex = 18;
            this.whitelistToggleButton.Text = "Enable";
            this.whitelistToggleButton.UseVisualStyleBackColor = true;
            this.whitelistToggleButton.Click += new System.EventHandler(this.whitelistToggleButton_Click);
            // 
            // whitelistLabel
            // 
            this.whitelistLabel.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.whitelistLabel.ForeColor = System.Drawing.Color.Red;
            this.whitelistLabel.Location = new System.Drawing.Point(92, 57);
            this.whitelistLabel.Name = "whitelistLabel";
            this.whitelistLabel.Size = new System.Drawing.Size(79, 23);
            this.whitelistLabel.TabIndex = 17;
            this.whitelistLabel.Text = "Disabled";
            this.whitelistLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // whitelistDescLabel
            // 
            this.whitelistDescLabel.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.whitelistDescLabel.Location = new System.Drawing.Point(12, 57);
            this.whitelistDescLabel.Name = "whitelistDescLabel";
            this.whitelistDescLabel.Size = new System.Drawing.Size(74, 23);
            this.whitelistDescLabel.TabIndex = 16;
            this.whitelistDescLabel.Text = "Whitelist:";
            this.whitelistDescLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // whitelistPanel
            // 
            this.whitelistPanel.BackColor = System.Drawing.SystemColors.Window;
            this.whitelistPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.whitelistPanel.Location = new System.Drawing.Point(12, 87);
            this.whitelistPanel.Name = "whitelistPanel";
            this.whitelistPanel.Size = new System.Drawing.Size(239, 340);
            this.whitelistPanel.TabIndex = 19;
            // 
            // whitelistAddTextBox
            // 
            this.whitelistAddTextBox.Location = new System.Drawing.Point(12, 433);
            this.whitelistAddTextBox.Name = "whitelistAddTextBox";
            this.whitelistAddTextBox.PlaceholderText = "IP: x.x.x.x";
            this.whitelistAddTextBox.Size = new System.Drawing.Size(159, 23);
            this.whitelistAddTextBox.TabIndex = 20;
            // 
            // whitelistAddButton
            // 
            this.whitelistAddButton.Location = new System.Drawing.Point(177, 432);
            this.whitelistAddButton.Name = "whitelistAddButton";
            this.whitelistAddButton.Size = new System.Drawing.Size(75, 25);
            this.whitelistAddButton.TabIndex = 21;
            this.whitelistAddButton.Text = "Add";
            this.whitelistAddButton.UseVisualStyleBackColor = true;
            this.whitelistAddButton.Click += new System.EventHandler(this.whitelistAddButton_Click);
            // 
            // blacklistAddButton
            // 
            this.blacklistAddButton.Location = new System.Drawing.Point(427, 432);
            this.blacklistAddButton.Name = "blacklistAddButton";
            this.blacklistAddButton.Size = new System.Drawing.Size(75, 25);
            this.blacklistAddButton.TabIndex = 27;
            this.blacklistAddButton.Text = "Add";
            this.blacklistAddButton.UseVisualStyleBackColor = true;
            this.blacklistAddButton.Click += new System.EventHandler(this.blacklistAddButton_Click);
            // 
            // blacklistAddTextBox
            // 
            this.blacklistAddTextBox.Location = new System.Drawing.Point(262, 433);
            this.blacklistAddTextBox.Name = "blacklistAddTextBox";
            this.blacklistAddTextBox.PlaceholderText = "IP: x.x.x.x";
            this.blacklistAddTextBox.Size = new System.Drawing.Size(159, 23);
            this.blacklistAddTextBox.TabIndex = 26;
            // 
            // blacklistPanel
            // 
            this.blacklistPanel.BackColor = System.Drawing.SystemColors.Window;
            this.blacklistPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.blacklistPanel.Location = new System.Drawing.Point(262, 87);
            this.blacklistPanel.Name = "blacklistPanel";
            this.blacklistPanel.Size = new System.Drawing.Size(239, 340);
            this.blacklistPanel.TabIndex = 25;
            // 
            // blacklistToggleButton
            // 
            this.blacklistToggleButton.Location = new System.Drawing.Point(427, 57);
            this.blacklistToggleButton.Name = "blacklistToggleButton";
            this.blacklistToggleButton.Size = new System.Drawing.Size(75, 23);
            this.blacklistToggleButton.TabIndex = 24;
            this.blacklistToggleButton.Text = "Enable";
            this.blacklistToggleButton.UseVisualStyleBackColor = true;
            this.blacklistToggleButton.Click += new System.EventHandler(this.blacklistToggleButton_Click);
            // 
            // blacklistLabel
            // 
            this.blacklistLabel.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.blacklistLabel.ForeColor = System.Drawing.Color.Red;
            this.blacklistLabel.Location = new System.Drawing.Point(342, 57);
            this.blacklistLabel.Name = "blacklistLabel";
            this.blacklistLabel.Size = new System.Drawing.Size(79, 23);
            this.blacklistLabel.TabIndex = 23;
            this.blacklistLabel.Text = "Disabled";
            this.blacklistLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // blacklistDescLabel
            // 
            this.blacklistDescLabel.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.blacklistDescLabel.Location = new System.Drawing.Point(262, 57);
            this.blacklistDescLabel.Name = "blacklistDescLabel";
            this.blacklistDescLabel.Size = new System.Drawing.Size(74, 23);
            this.blacklistDescLabel.TabIndex = 22;
            this.blacklistDescLabel.Text = "Blacklist:";
            this.blacklistDescLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ConnectionSettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(514, 469);
            this.Controls.Add(this.blacklistAddButton);
            this.Controls.Add(this.blacklistAddTextBox);
            this.Controls.Add(this.blacklistPanel);
            this.Controls.Add(this.blacklistToggleButton);
            this.Controls.Add(this.blacklistLabel);
            this.Controls.Add(this.blacklistDescLabel);
            this.Controls.Add(this.whitelistAddButton);
            this.Controls.Add(this.whitelistAddTextBox);
            this.Controls.Add(this.whitelistPanel);
            this.Controls.Add(this.whitelistToggleButton);
            this.Controls.Add(this.whitelistLabel);
            this.Controls.Add(this.whitelistDescLabel);
            this.Controls.Add(this.disconnectExcessCheckBox);
            this.Controls.Add(this.separatorLabel1);
            this.Controls.Add(this.maxConnectionsSelection);
            this.Controls.Add(this.maxConnectionsDescLabel);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ConnectionSettingsForm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Connection settings";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ConnectionSettingsForm_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.maxConnectionsSelection)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label maxConnectionsDescLabel;
        private System.Windows.Forms.NumericUpDown maxConnectionsSelection;
        private System.Windows.Forms.Label separatorLabel1;
        private System.Windows.Forms.CheckBox disconnectExcessCheckBox;
        private System.Windows.Forms.Button whitelistToggleButton;
        private System.Windows.Forms.Label whitelistLabel;
        private System.Windows.Forms.Label whitelistDescLabel;
        private System.Windows.Forms.Panel whitelistPanel;
        private System.Windows.Forms.TextBox whitelistAddTextBox;
        private System.Windows.Forms.Button whitelistAddButton;
        private System.Windows.Forms.Button blacklistAddButton;
        private System.Windows.Forms.TextBox blacklistAddTextBox;
        private System.Windows.Forms.Panel blacklistPanel;
        private System.Windows.Forms.Button blacklistToggleButton;
        private System.Windows.Forms.Label blacklistLabel;
        private System.Windows.Forms.Label blacklistDescLabel;
    }
}