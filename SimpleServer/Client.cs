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
        private Socket _socket;
        private NetworkStream _stream;
        public StreamReader _reader { get; private set; }
        //public StreamWriter _writer { get; private set; }

        public BinaryWriter _writer { get; private set; }
        public string nickName;

        public void Send(PacketType data)
        {
            MemoryStream ms = new MemoryStream();
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(ms, data);
            byte[] buffer = ms.GetBuffer();
            _writer.Write(buffer.Length);
            _writer.Write(buffer);
            _writer.Flush();
        }
        public Client(Socket socket)
        {
            _socket = socket;
            _stream = new NetworkStream(_socket);
            _reader = new StreamReader(_stream, System.Text.Encoding.UTF8);
           // _writer = new StreamWriter(_stream, System.Text.Encoding.UTF8);
            _writer = new BinaryWriter(_stream);
        }
        
        public void Close()
        {
            _socket.Close();
        }

    }
}
