using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.Timers;

namespace SimpleServer
{
    class SimpleServer
    {
        TcpListener tcpListner;
        List<Client> clientsList;
        List<string> nickNameList;
        Thread tUdpClientMethod;
        public void Server(string ipAddress, int port)
        {
            IPAddress ip = IPAddress.Parse(ipAddress);
            tcpListner = new TcpListener(ip, port);
            clientsList = new List<Client>();
            nickNameList = new List<string>();
        }

        public void Start()
        {
            tcpListner.Start();
            Console.WriteLine("Listener Started");
            while (true)
            {
                Socket _tcpSocket = tcpListner.AcceptSocket();
                Console.WriteLine("Connection Established");
                var client = new Client(_tcpSocket);
                clientsList.Add(client);

                Thread t = new Thread(new ParameterizedThreadStart(tcpClientMethod));
                t.Start(client);
            };
        }

        //TCP - UDP Tutorial
        private void tcpClientMethod(object clientObj)
        {
            Client client = (Client)clientObj;
            client.tcpConnect = true;

            while (client.tcpConnect)
            {
                Packet tcpPacketToHandle = client.tcpRead();
                HandlePacket(client, tcpPacketToHandle);
            }
            clientsList.Remove(client);
        }

        //TCP - UDP Tutorial
        private void udpClientMethod(object clientObj)
        {

            Client client = (Client)clientObj;

        
            
                System.Timers.Timer clientListTimer = new System.Timers.Timer((5 * 1000));
                clientListTimer.AutoReset = true;
                clientListTimer.Elapsed += (sender, e) => Timer_Elapsed(sender, e, client);
                clientListTimer.Start();
            

            while (client.udpConnect)
            {
                Packet udpPacketToHandle = client.udpRead();
                Console.WriteLine("Hits udpClientMethod");
                HandlePacket(client, udpPacketToHandle);
            }
            clientsList.Remove(client);
        }

        private void Timer_Elapsed(object sender, EventArgs e, object clientObj)
        {
            PushClientList();
        }

        public void PushClientList()
        {
            Packet connectedNicksPack = new Packet();
            connectedNicksPack = ConnectedNicknames(nickNameList);

            for(int i = 0; i < clientsList.Count; i++)
            {
                clientsList[i].UDPSend(connectedNicksPack);
            }
        }

        public ConnectedNicknames ConnectedNicknames(List<string> nicknames)
        {
            ConnectedNicknames connectedNicksPack = new ConnectedNicknames(nicknames);
            connectedNicksPack.nicknamesConnected = nicknames;
            return connectedNicksPack;
        }


        //TCP - UDP Tutorial
        private void HandlePacket(object clientObj, Packet packet)
        {
            Client client = (Client)clientObj;
            Packet packetToHandle = packet;

            switch (packetToHandle.type)
            {
                case PacketType.CHATMESSAGE:
                    ChatMessagePacket chatPack = (ChatMessagePacket)packetToHandle;
                    Console.WriteLine(chatPack.message);
                    chatPack.message = "[" + client.nickName + "] " + chatPack.message;
                    for (int i = 0; i < clientsList.Count; i++)
                    {
                        clientsList[i].tcpSend(chatPack);
                    }
                    break;
                case PacketType.NICKNAME:
                    NickNamePacket nicknamePacket = (NickNamePacket)packetToHandle;
                    client.nickName = nicknamePacket.nickName;
                    nickNameList.Add(nicknamePacket.nickName);
                    for (int i = 0; i < clientsList.Count; i++)
                    {
                        clientsList[i].UDPSend(nicknamePacket);
                    }
                    break;
                case PacketType.LOGIN:
                    Console.WriteLine("Login packet Created");
                    LoginPacket loginPacket = (LoginPacket)packetToHandle;
                    client.UdpConnect(loginPacket.endPoint);
                    tUdpClientMethod = new Thread(udpClientMethod);
                    tUdpClientMethod.Start(client);
                    break;
            }
        }

        public void Stop()
        {
            tcpListner.Stop();
        }
    }
}
