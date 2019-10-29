using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SimpleClient
{
    public partial class ClientForm : Form
    {
        delegate void UpdateChatWindowDelegate(string message);
        UpdateChatWindowDelegate _updateChatWindowDelegate;
        SimpleClient Client;

        public ClientForm()
        {
            InitializeComponent();
            _updateChatWindowDelegate = new UpdateChatWindowDelegate(UpdateChatWindow);
            Client = new SimpleClient();
        }

        public void UpdateChatWindow(string message)
        {
           if(inputChat.InvokeRequired)
            {
                Invoke(_updateChatWindowDelegate, message);
            }
            else
            {
                messageDisplay.Text += message;
                inputChat.SelectionStart = inputChat.Text.Length;
                inputChat.ScrollToCaret();
            }
         
        }

        private void ClientForm_Load(object sender, EventArgs e)
        {
            Client.Run();
        }

        private void ClientForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Client.Stop();
        }

        private void BtnSubmit_Click(object sender, EventArgs e)
        {
            Client.SendMessage(inputChat.Text);
        }
    }
}
