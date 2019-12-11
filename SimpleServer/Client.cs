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

        //UDP and TCP Task
        private Socket _udpSocket;

        private NetworkStream _stream;
        public BinaryWriter _writer;
        public BinaryReader _reader;
        public string nickName;

        BinaryFormatter _binaryFormatter; 

        public Client(Socket tcpSocket)
        {
            _tcpSocket = tcpSocket;

            //UDP and TCP Task
            _udpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

            _stream = new NetworkStream(_tcpSocket);
            _reader = new BinaryReader(_stream);
            _writer = new BinaryWriter(_stream);
            _binaryFormatter = new BinaryFormatter();
        }

        //UDP and TCP Task
        public void UdpConnect(EndPoint clientConnection)
        {
            _udpSocket.Connect(clientConnection);
            Packet loginPacket = new Packet();
            loginPacket = LoginPacket(_udpSocket.LocalEndPoint);
            tcpSend(loginPacket);
        }
        //UDP and TCP Task
        public LoginPacket LoginPacket(EndPoint endPoint)
        {
            LoginPacket loginPacket = new LoginPacket(endPoint);
            loginPacket.type = PacketType.LOGIN;
            loginPacket.endPoint = endPoint;
            return loginPacket;
        }

        //UDP and TCP Task
        void HandlePacket()
        {
            
        }

        //UDP and TCP Task
        void UDPSend(Packet packet)
        {
            MemoryStream ms = new MemoryStream();
            _binaryFormatter.Serialize(ms, packet);
            byte[] buffer = ms.GetBuffer();

            _writer.Write(buffer.Length);
            _writer.Write(buffer);
            _writer.Flush();

            _udpSocket.Send(buffer);
        }

        //UDP and TCP Task
        public Packet tcpRead()
        {
            int numberOfIncomingBytes;
            while((numberOfIncomingBytes = _reader.ReadInt32()) != 0)
            {
                MemoryStream ms = new MemoryStream();
                byte[] byteData = _reader.ReadBytes(numberOfIncomingBytes);
                return _binaryFormatter.Deserialize(ms) as Packet;
            }
            return null;
        }
        //UDP and TCP Task
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
    
        public void Close()
        {
            _tcpSocket.Close();
        }
    }
}
