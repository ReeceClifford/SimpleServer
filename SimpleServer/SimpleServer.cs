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

                Thread t = new Thread(new ParameterizedThreadStart(ClientMethod));
                t.Start(client);
            };
        }
 
        private void ClientMethod(Object clientObj)
        {
            Client client = (Client)clientObj;
            

            int noOfIncomingBytes = 0;
            while ((noOfIncomingBytes = client._reader.ReadInt32()) != 0)
            {
                MemoryStream ms = new MemoryStream();
                byte[] byteData = client._reader.ReadBytes(noOfIncomingBytes);

                ms.Write(byteData, 0, byteData.Length);
                ms.Position = 0;
                BinaryFormatter bf = new BinaryFormatter();
                Packet packet = bf.Deserialize(ms) as Packet;
                switch (packet.type)
                {
                    case PacketType.CHATMESSAGE:
                        ChatMessagePacket chatPack = (ChatMessagePacket)packet;
                        Console.WriteLine(chatPack.message);
                        chatPack.message = "[ " + client.nickName + " ] " + chatPack.message;
                        for (int i = 0; i < clientsList.Count; i++)
                        {
                            clientsList[i].tcpSend(packet);
                        }
                        break;
                    case PacketType.NICKNAME:
                        NickNamePacket nicknamePacket = (NickNamePacket)packet;
                        client.nickName = nicknamePacket.nickName;
                        break;
                }
            }
            clientsList.Remove(client);
        }

        public void Stop()
        {
            tcpListner.Stop();
        }
    }

}
