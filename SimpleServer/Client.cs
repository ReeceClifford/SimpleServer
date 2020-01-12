using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;

namespace SimpleServer
{
    class Client
    {
        private Socket _tcpSocket;
        private Socket _udpSocket;

        private NetworkStream _stream;
        public BinaryWriter _writer;
        public BinaryReader _reader;
        public BinaryFormatter _binaryFormatter;

        

        public string nickName;

        public bool tcpConnect;
        public bool udpConnect;

        public Client(Socket tcpSocket)
        {
            _tcpSocket = tcpSocket;

            _udpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

            _stream = new NetworkStream(_tcpSocket);
            _reader = new BinaryReader(_stream);
            _writer = new BinaryWriter(_stream);
            _binaryFormatter = new BinaryFormatter();
        }

        public void UdpConnect(EndPoint clientConnection)
        {
            _udpSocket.Connect(clientConnection);
            Packet loginPacket = new Packet();
            loginPacket = LoginPacket(_udpSocket.LocalEndPoint);
            tcpSend(loginPacket);
            udpConnect = true;
        }

        public LoginPacket LoginPacket(EndPoint endPoint)
        {
            LoginPacket loginPacket = new LoginPacket(endPoint);
            loginPacket.type = PacketType.LOGIN;
            loginPacket.endPoint = endPoint;
            return loginPacket;
        }

        public Packet tcpRead()
        {
            int numberOfIncomingBytes = 0;
            while (tcpConnect)
            {
                while ((numberOfIncomingBytes = _reader.ReadInt32()) != 0)
                {
                    byte[] byteData = _reader.ReadBytes(numberOfIncomingBytes);
                    MemoryStream ms = new MemoryStream();
                    ms.Write(byteData, 0, byteData.Length);
                    ms.Position = 0;
                    return _binaryFormatter.Deserialize(ms) as Packet;
                }
            }
            return new Packet();
        }

        public Packet udpRead()
        {
            int numberOfIncomingBytes = 0;
            byte[] bytes = new byte[256];
            while ((numberOfIncomingBytes = _udpSocket.Receive(bytes)) != 0)
            {
                MemoryStream ms = new MemoryStream(bytes);
                return _binaryFormatter.Deserialize(ms) as Packet;
            }
            return new Packet();
        }

        public void tcpSend(Packet data)
        {
            MemoryStream ms = new MemoryStream();
            _binaryFormatter.Serialize(ms, data);
            byte[] buffer = ms.GetBuffer();

            _writer.Write(buffer.Length);
            _writer.Write(buffer);
            _writer.Flush();
        }

       public void UDPSend(Packet packet)
        {
            MemoryStream ms = new MemoryStream();
            _binaryFormatter.Serialize(ms, packet);
            byte[] buffer = ms.GetBuffer();

            _udpSocket.Send(buffer);
        }

        public void Close()
        {
            _tcpSocket.Close();
            _udpSocket.Close();
        }
    }
}
