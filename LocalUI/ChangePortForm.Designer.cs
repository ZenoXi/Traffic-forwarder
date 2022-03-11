
namespace LocalUI
{
    partial class ChangePortForm
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
            this.portInput = new System.Windows.Forms.TextBox();
            this.setPortButton = new System.Windows.Forms.Button();
            this.cancelPortButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // portInput
            // 
            this.portInput.Location = new System.Drawing.Point(10, 10);
            this.portInput.MaxLength = 5;
            this.portInput.Name = "portInput";
            this.portInput.PlaceholderText = "Enter port (1-65535)";
            this.portInput.Size = new System.Drawing.Size(130, 23);
            this.portInput.TabIndex = 0;
            this.portInput.WordWrap = false;
            this.portInput.TextChanged += new System.EventHandler(this.portInput_TextChanged);
            this.portInput.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.portInput_KeyPress);
            // 
            // setPortButton
            // 
            this.setPortButton.Location = new System.Drawing.Point(10, 39);
            this.setPortButton.Name = "setPortButton";
            this.setPortButton.Size = new System.Drawing.Size(60, 23);
            this.setPortButton.TabIndex = 1;
            this.setPortButton.Text = "Set";
            this.setPortButton.UseVisualStyleBackColor = true;
            this.setPortButton.Click += new System.EventHandler(this.setPortButton_Click);
            // 
            // cancelPortButton
            // 
            this.cancelPortButton.Location = new System.Drawing.Point(80, 39);
            this.cancelPortButton.Name = "cancelPortButton";
            this.cancelPortButton.Size = new System.Drawing.Size(60, 23);
            this.cancelPortButton.TabIndex = 2;
            this.cancelPortButton.Text = "Cancel";
            this.cancelPortButton.UseVisualStyleBackColor = true;
            this.cancelPortButton.Click += new System.EventHandler(this.cancelPortButton_Click);
            // 
            // ChangePortForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(149, 69);
            this.ControlBox = false;
            this.Controls.Add(this.cancelPortButton);
            this.Controls.Add(this.setPortButton);
            this.Controls.Add(this.portInput);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "ChangePortForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Change port";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox portInput;
        private System.Windows.Forms.Button setPortButton;
        private System.Windows.Forms.Button cancelPortButton;
    }
}