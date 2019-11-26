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
        NetworkStream stream;
        BinaryReader reader;
        BinaryWriter writer;
        BinaryFormatter binaryFormatter;
        ClientForm messageForm;
        Thread readerThread;

        public object _clients { get; private set; }

        public void SimpleClientMain()
        {
            tcpClient = new TcpClient();
            messageForm = new ClientForm(this);
        }

        public bool Connect(string ipAddress, int port)
        {
            try
            {
                tcpClient.Connect(ipAddress, port);
                stream = tcpClient.GetStream();
                reader = new BinaryReader(stream, System.Text.Encoding.UTF8);
                writer = new BinaryWriter(stream, System.Text.Encoding.UTF8);
                binaryFormatter = new BinaryFormatter();
                readerThread = new Thread(Listen);

                Application.Run(messageForm);

                Console.WriteLine("Connected");
            }
            catch 
            {
                Console.WriteLine("Error Connecting");
                return false;
            }
            return true;
        }

        public void Run()
        {
            readerThread.Start();
        }

        private void Listen()
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
                        chatPacket.message =  chatPacket.message;
                        messageForm.UpdateChatWindow(chatPacket.message);
                        break;

                    case PacketType.NICKNAME:
                        NickNamePacket nicknamePacket = (NickNamePacket)packet;
                        Console.WriteLine(nicknamePacket.nickName);
                        
                        break;
                }
            }
        }
     

        public void Send(Packet data)
        {
            MemoryStream ms = new MemoryStream();
            binaryFormatter.Serialize(ms, data);
            byte[] buffer = ms.GetBuffer();

            writer.Write(buffer.Length);
            writer.Write(buffer);
            writer.Flush();
        }

        public void SendMessage(string message)
        {
            string userInput = message;
            if (userInput != "")
            {
                writer.Write(userInput);
                writer.Flush();
            };
        }// Currently Not in use.

        public void Stop()
        {
            readerThread.Abort();
            tcpClient.Close();
        }
    }
}
