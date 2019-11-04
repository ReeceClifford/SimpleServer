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

        public ClientForm(object _client)
        {
            InitializeComponent();
            _updateChatWindowDelegate = new UpdateChatWindowDelegate(UpdateChatWindow);
            Client = (SimpleClient) _client;
            inputChat.Select();
        }

        public void UpdateChatWindow(string message)
        {
           if(messageDisplay.InvokeRequired)
            {
                Invoke(_updateChatWindowDelegate, message);
            }
            else
            {
                messageDisplay.Text += message + "\n";
                messageDisplay.SelectionStart = messageDisplay.Text.Length;
                messageDisplay.ScrollToCaret();
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
            inputChat.Text = null;
        }
    }
}
