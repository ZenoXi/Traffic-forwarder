
namespace LocalUI
{
    partial class ChangeIPForm
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
            this.cancelIpButton = new System.Windows.Forms.Button();
            this.setIpButton = new System.Windows.Forms.Button();
            this.ipInput = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // cancelIpButton
            // 
            this.cancelIpButton.Location = new System.Drawing.Point(80, 39);
            this.cancelIpButton.Name = "cancelIpButton";
            this.cancelIpButton.Size = new System.Drawing.Size(60, 23);
            this.cancelIpButton.TabIndex = 2;
            this.cancelIpButton.Text = "Cancel";
            this.cancelIpButton.UseVisualStyleBackColor = true;
            this.cancelIpButton.Click += new System.EventHandler(this.cancelIpButton_Click);
            // 
            // setIpButton
            // 
            this.setIpButton.Location = new System.Drawing.Point(10, 39);
            this.setIpButton.Name = "setIpButton";
            this.setIpButton.Size = new System.Drawing.Size(60, 23);
            this.setIpButton.TabIndex = 1;
            this.setIpButton.Text = "Set";
            this.setIpButton.UseVisualStyleBackColor = true;
            this.setIpButton.Click += new System.EventHandler(this.setIpButton_Click);
            // 
            // ipInput
            // 
            this.ipInput.Location = new System.Drawing.Point(10, 10);
            this.ipInput.MaxLength = 15;
            this.ipInput.Name = "ipInput";
            this.ipInput.Size = new System.Drawing.Size(130, 23);
            this.ipInput.TabIndex = 0;
            this.ipInput.WordWrap = false;
            this.ipInput.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.ipInput_KeyPress);
            // 
            // ChangeIPForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(151, 69);
            this.ControlBox = false;
            this.Controls.Add(this.cancelIpButton);
            this.Controls.Add(this.setIpButton);
            this.Controls.Add(this.ipInput);
            this.Name = "ChangeIPForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Enter server IP";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button cancelPortButton;
        private System.Windows.Forms.Button setIpButton;
        private System.Windows.Forms.TextBox ipInput;
        private System.Windows.Forms.Button cancelIpButton;
    }
}