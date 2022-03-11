using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace LocalUI
{
    public partial class ChangePortForm : Form
    {
        public string port = "";
        private bool undoCalled = false;

        public ChangePortForm(string currentPort)
        {
            InitializeComponent();

            if (ValidatePort(currentPort) == 0)
            {
                port = "";
                portInput.Text = "";
            }
            else
            {
                port = currentPort;
                portInput.Text = currentPort;
            }
        }

        private void setPortButton_Click(object sender, EventArgs e)
        {
            CloseSet();
        }

        private void cancelPortButton_Click(object sender, EventArgs e)
        {
            CloseCancel();
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape)
            {
                CloseCancel();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void portInput_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = true;
                CloseSet();
            }

            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
                return;
            }
        }

        private void portInput_TextChanged(object sender, EventArgs e)
        {
            if (undoCalled)
            {
                undoCalled = false;
                return;
            }

            // Check if port is valid
            if (portInput.Text.Length == 0)
            {
                port = "";
                return;
            }
            if (portInput.Text.StartsWith('0'))
            {
                UndoPortChange();
                return;
            }
            if (ValidatePort(portInput.Text) == 0)
            {
                UndoPortChange();
                return;
            }

            port = portInput.Text;
        }

        private void UndoPortChange()
        {
            undoCalled = true;
            portInput.Text = port;
        }

        /// <param name="portString"></param>
        /// <returns>0 if port is invalid, the port (1-65535) otherwise</returns>
        private int ValidatePort(string portString)
        {
            int intPort;
            if (!int.TryParse(portString, out intPort))
            {
                return 0;
            }
            else if (intPort < 1 || intPort > 65535)
            {
                return 0;
            }
            return intPort;
        }

        private void CloseSet()
        {
            port = portInput.Text;
            Close();
        }

        private void CloseCancel()
        {
            port = "";
            Close();
        }
    }
}
