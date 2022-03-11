using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace LocalUI
{
    public partial class ChangeIPForm : Form
    {
        public string ip;
        private bool _cancelable;

        public ChangeIPForm(string currentIp, bool cancelable)
        {
            InitializeComponent();

            ipInput.Text = currentIp;
            _cancelable = cancelable;
            if (!_cancelable)
            {
                cancelIpButton.Enabled = false;
            }
        }

        private void setIpButton_Click(object sender, EventArgs e)
        {
            CloseSet();
        }

        private void cancelIpButton_Click(object sender, EventArgs e)
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

        private void ipInput_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = true;
                CloseSet();
            }
        }

        private void CloseSet()
        {
            if (Program.options.IpValid(ipInput.Text))
            {
                ip = ipInput.Text;
                Close();
            }
            else
            {
                ipInput.BackColor = Color.FromArgb(255, 128, 128);
            }
        }

        private void CloseCancel()
        {
            if (_cancelable)
            {
                ip = "";
                Close();
            }
        }
    }
}
