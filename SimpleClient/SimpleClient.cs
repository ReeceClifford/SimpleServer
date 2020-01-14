using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.IO;
using System.Windows.Forms;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;

namespace SimpleClient
{
    class SimpleClient
    {
        TcpClient tcpClient;
        Thread tUdpClientMethod;
        UdpClient udpClient;

        NetworkStream stream;
        BinaryReader reader;
        BinaryWriter writer;
        BinaryFormatter binaryFormatter;
        ClientForm messageForm;
        Thread readerThread;

        public object _clients { get; private set; }

        bool tcpClientConnect;
        bool udpClientConnect;

        public void SimpleClientMain()
        {
            messageForm = new ClientForm(this);
        }

        public bool Connect(string ipAddress, int port)
        {
            try
            {
                tcpClient = new TcpClient();
                udpClient = new UdpClient();
                tcpClient.Connect(ipAddress, port);
                udpClient.Connect(ipAddress, port);
                stream = tcpClient.GetStream();
                reader = new BinaryReader(stream, System.Text.Encoding.UTF8);
                writer = new BinaryWriter(stream, System.Text.Encoding.UTF8);
                binaryFormatter = new BinaryFormatter();
                tcpClientConnect = true;

                Packet loginPacket = LoginPacket(udpClient.Client.LocalEndPoint);
                TCPClientSend(loginPacket);

                readerThread = new Thread(TCPServerResponse);
                
                Application.Run(messageForm);

                Console.WriteLine("Connected");
            }
            catch 
            {
                Console.WriteLine("Error Connecting TCP");
                return false;
            }
            return true;
        }

        public void Run()
        {
            try
            {
                readerThread.Start();
            }
            catch(Exception e)
            {
                Console.WriteLine("Error " + e.Message);
            }

        }

        public LoginPacket LoginPacket(EndPoint endPoint)
        {
            LoginPacket loginPacket = new LoginPacket(endPoint);
            loginPacket.type = PacketType.LOGIN;
            loginPacket.endPoint = endPoint;
            return loginPacket;
        }

        private void TCPServerResponse() 
        {
            while (tcpClientConnect)
            {
                int numberOfIncomingBytes;
                while ((numberOfIncomingBytes = reader.ReadInt32()) != 0)
                {
                    MemoryStream ms = new MemoryStream();
                    byte[] byteData = reader.ReadBytes(numberOfIncomingBytes);

                    ms.Write(byteData, 0, byteData.Length);
                    ms.Position = 0;

                    Packet packet = binaryFormatter.Deserialize(ms) as Packet;
                    Console.WriteLine("Packet Deserialized");
                    switch (packet.type)
                    {
                        case PacketType.CHATMESSAGE:
                            ChatMessagePacket chatPacket = (ChatMessagePacket)packet;
                            Console.WriteLine(chatPacket.message);
                            messageForm.UpdateChatWindow(chatPacket.message);
                            break;
                        case PacketType.LOGIN:
                            Console.WriteLine("Login packet Created");
                            LoginPacket tcpLoginPacket = (LoginPacket)packet;
                            udpClient.Client.Connect(tcpLoginPacket.endPoint);
                            tUdpClientMethod = new Thread(UDPServerResponse);
                            tUdpClientMethod.Start();
                            udpClientConnect = true;
                            break;

                    }
                }
            }
        }

        private void UDPServerResponse() 
        {
            try
            {
                while(udpClientConnect)
                {
                    Packet udpReadPacket = udpClientRead();
                    switch (udpReadPacket.type)
                    {
                        //case PacketType.NICKNAME:
                        //    NickNamePacket nicknamePacket = (NickNamePacket)udpReadPacket;
                        //    Console.WriteLine(nicknamePacket.nickName);
                        //    messageForm.UpdateClientList(nicknamePacket.nickName);
                        //    break;
                        case PacketType.CLIENTSCONNECTED:
                           
                            ConnectedNicknames connectedNicknamePack = (ConnectedNicknames)udpReadPacket;
                            Console.WriteLine("Clients connected are " + connectedNicknamePack.nicknamesConnected);
                            messageForm.UpdateClientListBox(connectedNicknamePack.nicknamesConnected);
                            break;
                        //case PacketType.DISCONNECTEDCLIENT:
                        //    DisconnectedNicknames disconnectedNicknames = (DisconnectedNicknames)udpReadPacket;
                        //    messageForm.ClearUpdateClientListBox(disconnectedNicknames.disconnectedNickname);

                        //    break;
                    }
                }
            }
            catch(Exception e)
            {
                
            }
        }

        public void TCPClientSend(Packet data) 
        {
            MemoryStream ms = new MemoryStream();
            binaryFormatter.Serialize(ms, data);
            byte[] buffer = ms.GetBuffer();

            writer.Write(buffer.Length);
            writer.Write(buffer);
            writer.Flush();
        }

       public void UDPClientSend(Packet packet)
        {
            MemoryStream ms = new MemoryStream();
            binaryFormatter.Serialize(ms, packet);
            byte[] buffer = ms.GetBuffer();

            udpClient.Send(buffer, buffer.Length);
        }

        public Packet udpClientRead()
        {
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 4444);
            byte[] bytes = new byte[256];
            bytes = udpClient.Receive(ref endPoint);
            MemoryStream ms = new MemoryStream(bytes);
            return binaryFormatter.Deserialize(ms) as Packet;
        }

        public void Stop()
        {
            readerThread.Abort();
            tcpClient.Close();
            udpClient.Close();
         
            //messageForm.Close();
        }
    }
}
