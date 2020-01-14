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

        delegate void UpdateConnectedClientsDelegate(string nicknames);
        UpdateConnectedClientsDelegate _updateConnectedClientsDelegate;

        SimpleClient Client;

        public ClientForm(object _client)
        {
            InitializeComponent();
            _updateChatWindowDelegate = new UpdateChatWindowDelegate(UpdateChatWindow);
            _updateConnectedClientsDelegate = new UpdateConnectedClientsDelegate(UpdateClientListBox);
            Client = (SimpleClient)_client;
            inputChat.Select();
        }

        private void ClientForm_Load(object sender, EventArgs e)
        {
            Client.Run();
        }

        private void BtnSubmit_Click(object sender, EventArgs e)
        {
            Client.TCPClientSend(new ChatMessagePacket(inputChat.Text));
            inputChat.Text = null;
        }

        public void UpdateChatWindow(string message)
        {
            if (chatRelay.InvokeRequired)
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

        public void UpdateClientListBox(string nickname)
        {
            if (clientConnectLB.InvokeRequired)
            {
                Invoke(_updateConnectedClientsDelegate, nickname);
            }

            else if (nickname != null)
            {

                if (!clientConnectLB.Items.Contains(nickname))
                    clientConnectLB.Items.Add(nickname);

            }
        }
        public void ClearUpdateClientListBox(string nickname)
        {

            for (int i = clientConnectLB.Items.Count - 1; i >= 0; --i)
            {
                string removelistitem = nickname;
                if (clientConnectLB.Items[i].ToString().Contains(removelistitem))
                {
                    clientConnectLB.Items.RemoveAt(i);
                }
            }
        }
        private void ClientForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Client.Stop();
        }

        private void connectButton_Click(object sender, EventArgs e)
        {
            if (nicknameTextBox.Text != "")
            {
                chatRelay.Text = "Weclome to the Chat room\n";
                Client.TCPClientSend(new NickNamePacket(nicknameTextBox.Text));
                nicknameLabel.Visible = false;
                nicknameTextBox.Visible = false;
                inputChat.Visible = true;
                btnSubmit.Visible = true;
                connectButton.Visible = false;
            }
            else
            {
                MessageBox.Show("NICKNAME MUST BE CHOSEN");
            }
        }

        private void disconnectButton_Click(object sender, EventArgs e)
        {
            bool clientDcPressed = true;
            Client.UDPClientSend(new DisconnectPacket(clientDcPressed));
            //Client.TCPClientSend(new DisconnectedNicknames(nicknameTextBox.Text));
            nicknameLabel.Visible = true;
            nicknameTextBox.Visible = true;
            inputChat.Visible = false;
            btnSubmit.Visible = false;
            connectButton.Visible = true;
           
        }
    }
}



