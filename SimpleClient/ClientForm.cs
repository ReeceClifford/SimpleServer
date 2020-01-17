using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.VisualBasic;

namespace SimpleClient
{
    public partial class ClientForm : Form
    {
        delegate void UpdateChatWindowDelegate(string message);
        UpdateChatWindowDelegate _updateChatWindowDelegate;

        delegate void UpdateConnectedClientsDelegate(List<string> nicknames);
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

        public void UpdateClientListBox(List<string> nicknameList)
        {
            if (clientConnectLB.InvokeRequired)
            {
                Invoke(_updateConnectedClientsDelegate, nicknameList);
            }
            else
            {
                clientConnectLB.Items.Clear();          
                for(int i = 0; i < nicknameList.Count; i++ )
                {
                    clientConnectLB.Items.Add(nicknameList[i]);
                }
            }
        }      

        private void ClientForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            bool clientDcPressed = true;
            Client.UDPClientSend(new DisconnectPacket(clientDcPressed));
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
                tankGameBtn.Visible = true;
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
            clientConnectLB.Items.Clear();
            nicknameLabel.Visible = true;
            nicknameTextBox.Visible = true;
            inputChat.Visible = false;
            btnSubmit.Visible = false;
            connectButton.Visible = true;
            tankGameBtn.Visible = false;
        }

        private void tankGameBtn_Click(object sender, EventArgs e)
        {
            Client.LoadMainGame();
        }

        //private void clientConnectLB_DoubleClick(object sender, EventArgs e)
        //{
        //    string targetNickname = clientConnectLB.GetItemText(clientConnectLB.SelectedItem);
        //    Client.TCPClientSend(new PrivateMessageRequest(targetNickname));
        //    PrivateMessaging pmForm = new PrivateMessaging(sender);

        //    pmForm.Show();
        //}
    }
}



