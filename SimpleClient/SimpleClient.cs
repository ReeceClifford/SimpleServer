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
using System.Drawing;

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
       

        TankGame mainGame;

        Thread gameThread;

        public object _clients { get; private set; }

        bool tcpClientConnect;
        bool udpClientConnect;

        public void SimpleClientMain()
        {
            messageForm = new ClientForm(this);
           
            //Game Stuff
            mainGame = new TankGame(this);
        }
        public void LoadMainGame()
        {
            mainGame.ShowDialog();
        }

        // Private Messaging 
        //        public void PrivateMessageMain()
        //{
        //    pmThread = new Thread(PrivateMessageMain);
        //    pmForm = new PrivateMessaging(this);
        //    Application.Run(pmForm);
        //}

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
                gameThread = new Thread(LoadMainGame);
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

        private void TCPServerResponse() //TCP Read is also here
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
                        // Private Messaging
                                                        //case PacketType.PRIVATEMESSAGEREQUEST:
                                //    Console.WriteLine("Pm Recieved");
                                //    PrivateMessageRequest privateMessagePack = (PrivateMessageRequest)packet;
                                //    PrivateMessageMain();
                                //    break;
                                //case PacketType.PRIVATEMESSAGETOSEND:
                                //    PrivateMessageToSend privateMessageToSend = (PrivateMessageToSend)packet;
                                //    pmForm.UpdatePrivateMessageChatWindow(privateMessageToSend.privateMessageToSend);
                                //    break;
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
                    Packet udpReadPacket = UDPClientRead();
                    switch (udpReadPacket.type)
                    {
                        case PacketType.NICKNAMESCONNECTED:
                            ConnectedNicknames connectedNicknamePack = (ConnectedNicknames)udpReadPacket;
                            Console.WriteLine("Clients connected are " + connectedNicknamePack.nicknamesConnected);
                            messageForm.UpdateClientListBox(connectedNicknamePack.nicknamesConnected);
                            break;
                        case PacketType.GAMEINFORMATION:
                            GameInfoUpdate gameInfoUpdatePacket = (GameInfoUpdate)udpReadPacket;
                            mainGame.UpdateDictionaryInfo(gameInfoUpdatePacket.clientGameTankPacket);
                            mainGame.ForcePaint();
                            break;
                        case PacketType.GAMESPRITE:
                            GameInfoSpriteUpdate gameInfoSpriteUpdate = (GameInfoSpriteUpdate)udpReadPacket;
                            mainGame.UpdateSpriteInfo(gameInfoSpriteUpdate.cliengGameSpriteInfo);
                            break;
                        case PacketType.GAMEBOMBINFORMATION:
                            GameInfoBombUpdate gameInfoBombUpdatePacket = (GameInfoBombUpdate)udpReadPacket;
                            mainGame.UpdateBombDictionaryInfo(gameInfoBombUpdatePacket.clientGameBombPacket);
                            mainGame.ForcePaint();
                            break;
                        case PacketType.GAMEBOMBSPRITE:
                            GameInfoBombSpriteUpdate gameInfoBombSpriteUpdate = (GameInfoBombSpriteUpdate)udpReadPacket;
                            mainGame.UpdateBombSpriteInfo(gameInfoBombSpriteUpdate.cliengGameBombSpriteInfo);
                            break;
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

        public Packet UDPClientRead()
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
