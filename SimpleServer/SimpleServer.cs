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
        List<Client> _clients;

        public void Server(string ipAddress, int port)
        {
            IPAddress ip = IPAddress.Parse(ipAddress);
            tcpListner = new TcpListener(ip, port);
            _clients = new List<Client>();
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
                _clients.Add(client);

                Thread t = new Thread(new ParameterizedThreadStart(ClientMethod));
                t.Start(client);
            };
        }

        public void Stop()
        {
            tcpListner.Stop();
        }
  
        private void ClientMethod(Object clientObj)
        {
            String receivedMessage;
            Client client = (Client)clientObj;

            client._writer.WriteLine("Connection Made. \nPlease Enter a Nickname");
            client._writer.Flush();

            client.nickName = client._reader.ReadLine();
            Console.WriteLine("Client assigned Nickname " + client.nickName);
            
            while ((receivedMessage = client._reader.ReadLine()) != null && client.nickName != "")
            {
                Console.WriteLine(receivedMessage);

                for (int i = 0; i < _clients.Count; i++)
                {
                    //Error Checking
                    // if (_clients[i] != client)
                    //{
                    _clients[i]._writer.WriteLine("< " + client.nickName + " > " + receivedMessage);
                    _clients[i]._writer.Flush();
                    // }
                }
            }
            _clients.Remove(client);
        }
    }
}
