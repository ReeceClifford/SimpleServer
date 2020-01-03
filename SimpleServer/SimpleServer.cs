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

namespace SimpleServer
{
    class SimpleServer
    {
        TcpListener tcpListner;
        List<Client> clientsList;
        Thread tUdpClientMethod;
        public void Server(string ipAddress, int port)
        {
            IPAddress ip = IPAddress.Parse(ipAddress);
            tcpListner = new TcpListener(ip, port);
            clientsList = new List<Client>();
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
                        //int noOfIncomingBytes = 0;

            //while (true)
            //{
            //    Packet updReadPacket = client.udpRead();
            //    switch (updReadPacket.type)
            //    {
            //        case PacketType.CHATMESSAGE:
            //            ChatMessagePacket chatPack = (ChatMessagePacket)updReadPacket;
            //            Console.WriteLine(chatPack.message);
            //            chatPack.message = "[" + client.nickName + "] " + chatPack.message;
            //            for (int i = 0; i < clientsList.Count; i++)
            //            {
            //                clientsList[i].tcpSend(updReadPacket);
            //            }
            //            break;
            //        case PacketType.NICKNAME:
            //            NickNamePacket nicknamePacket = (NickNamePacket)updReadPacket;
            //            client.nickName = nicknamePacket.nickName;
            //            break;
            //        case PacketType.LOGIN:
            //            LoginPacket loginPacket = (LoginPacket)updReadPacket;
            //            client.UdpConnect(loginPacket.endPoint);
            //            //HandlePacket(client, loginPacket.endPoint);
            //            break;
            //    }
            //}//Old Method
            while (true)
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
                                                    //int noOfIncomingBytes = 0;

                                        //while ((noOfIncomingBytes = client._reader.ReadInt32()) != 0)
                                        //{
                                        //    Packet updReadPacket = client.udpRead();
                                        //    switch (updReadPacket.type)
                                        //    {
                                        //        case PacketType.CHATMESSAGE:
                                        //            ChatMessagePacket chatPack = (ChatMessagePacket)updReadPacket;
                                        //            Console.WriteLine(chatPack.message);
                                        //            chatPack.message = "[" + client.nickName + "] " + chatPack.message;
                                        //            for (int i = 0; i < clientsList.Count; i++)
                                        //            {
                                        //                clientsList[i].tcpSend(updReadPacket);
                                        //            }
                                        //            break;
                                        //        case PacketType.NICKNAME:
                                        //            NickNamePacket nicknamePacket = (NickNamePacket)updReadPacket;
                                        //            client.nickName = nicknamePacket.nickName;
                                        //            break;
                                        //        case PacketType.LOGIN:
                                        //            LoginPacket loginPacket = (LoginPacket)updReadPacket;
                                        //            client.UdpConnect(loginPacket.endPoint);
                                        //            //HandlePacket(client, loginPacket.endPoint);
                                        //            break;
                                        //    }
                                        //} //Old Method

            while (true)
            {
                Packet udpPacketToHandle = client.udpRead();
                Console.WriteLine("Hits udpClientMethod");
                HandlePacket(client, udpPacketToHandle);
            }
            clientsList.Remove(client);
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
                        clientsList[i].tcpSend(packetToHandle);
                    }
                    break;
                case PacketType.NICKNAME:
                    NickNamePacket nicknamePacket = (NickNamePacket)packetToHandle;
                    client.nickName = nicknamePacket.nickName;
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
