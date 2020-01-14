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

        delegate void UpdateConnectedClientsDelegate(List<string> nickname);
        UpdateConnectedClientsDelegate _updateConnectedClientsDelegate;

        SimpleClient Client;

        public ClientForm(object _client)
        {
            InitializeComponent();
            _updateChatWindowDelegate = new UpdateChatWindowDelegate(UpdateChatWindow);
            _updateConnectedClientsDelegate = new UpdateConnectedClientsDelegate(UpdateClientList);
            Client = (SimpleClient) _client;
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
        public void UpdateClientList(List<string> nickname)
        {
            if (activeClientList.InvokeRequired)
            {
                Invoke(_updateConnectedClientsDelegate, nickname);
            }
            else
            {
                activeClientList.Text = null;
                for(int i = 0; i < nickname.Count; i++)
                {
                    Console.WriteLine("Nickname before writing to client list is " + nickname);
                    activeClientList.Text += nickname[i] + "\n";
                    activeClientList.SelectionStart = activeClientList.Text.Length;
                    activeClientList.ScrollToCaret();
                }
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
                Client.TCPClientSend(new NickNamePacket(nicknameTextBox.Text));
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
