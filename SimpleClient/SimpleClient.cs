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
        //StreamReader reader;
        //StreamWriter writer;
        BinaryReader reader;
        BinaryWriter writer;
        BinaryFormatter bf;

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
                bf = new BinaryFormatter();
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

        public void SendMessage(string message)
        {
            string userInput = message;
            if (userInput != "")
            {
                writer.Write(userInput);
                writer.Flush();
            };
        }

        public void Stop()
        {
            readerThread.Abort();
            tcpClient.Close();
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

                Packet packet = bf.Deserialize(ms) as Packet;
                switch (packet.type)
                {
                    case PacketType.CHATMESSAGE:
                        ChatMessagePacket chatPacket = (ChatMessagePacket)packet;
                        Console.WriteLine(chatPacket.message);
                        chatPacket.message = chatPacket.message;
                        Send(chatPacket);
                        break;
                }
            }
        }

        public void Send(Packet data)
        {
            MemoryStream ms = new MemoryStream();
            bf.Serialize(ms, data);
            byte[] buffer = ms.GetBuffer();

            writer.Write(buffer.Length);
            writer.Write(buffer);
            writer.Flush();
        }
    }
}
