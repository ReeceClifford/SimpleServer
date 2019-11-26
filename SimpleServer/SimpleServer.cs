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
        List<Client> clients;

        public void Server(string ipAddress, int port)
        {
            IPAddress ip = IPAddress.Parse(ipAddress);
            tcpListner = new TcpListener(ip, port);
            clients = new List<Client>();
        }

        public void Start()
        {
            tcpListner.Start();
            Console.WriteLine("Listener Started");
            while (true)
            {
                Socket _socket = tcpListner.AcceptSocket();
                Console.WriteLine("Connection Established");

                var client = new Client(_socket);
                clients.Add(client);

                Thread t = new Thread(new ParameterizedThreadStart(ClientMethod));
                t.Start(client);
            };
        }
 
 
        private void ClientMethod(Object clientObj)
        {
            Client client = (Client)clientObj;
            int numberOfIncomingBytes;
            while ((numberOfIncomingBytes = client._reader.ReadInt32()) != 0)
            {
                MemoryStream ms = new MemoryStream();
                byte[] byteData = client._reader.ReadBytes(numberOfIncomingBytes);

                ms.Write(byteData, 0, byteData.Length);
                ms.Position = 0;

                BinaryFormatter bf = new BinaryFormatter();
                Packet packet = bf.Deserialize(ms) as Packet;
                for (int i = 0; i < clients.Count; i++)
                {
                    clients[i].Send(packet);
                }
            }
            clients.Remove(client);
        }

        public void Stop()
        {
            tcpListner.Stop();
        }
    }

}
