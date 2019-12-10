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

        private void ClientForm_Load(object sender, EventArgs e)
        {
            Client.Run();
        }

        private void BtnSubmit_Click(object sender, EventArgs e)
        {
                Client.Send(new ChatMessagePacket(inputChat.Text));
                inputChat.Text = null;
        }

        public void UpdateChatWindow(string message)
        {
           if(chatRelay.InvokeRequired)
            {
                Invoke(_updateChatWindowDelegate, message);
            }
            else
            {
                chatRelay.Text += message + "\n";
                chatRelay.SelectionStart = chatRelay.Text.Length;
                chatRelay.ScrollToCaret();
            }
        }

        private void ClientForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Client.Stop();
        }

        private void ConnectDcBtn_Click(object sender, EventArgs e)
        {
            if(nicknameTextBox.Text != "" && connectDcBtn.Text != "Disconnect")
            {
                chatRelay.Text = "Weclome to the Chat room\n";
                Client.Send(new NickNamePacket(nicknameTextBox.Text));
                nicknameLabel.Visible = false;
                nicknameTextBox.Visible = false;
                inputChat.Visible = true;
                btnSubmit.Visible = true;
                connectDcBtn.Text = "Disconnect";
            }
            else
            {
                Client.Stop();
            }
        }
    }
}
