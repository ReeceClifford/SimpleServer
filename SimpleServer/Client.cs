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

        
        public void Send(Packet data)
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
            _udpSocket.Close();
        }
    }
}
