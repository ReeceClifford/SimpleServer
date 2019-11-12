using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.IO;


namespace SimpleServer
{
    class SimpleServer
    {
        TcpListener tcpListner;
        List<Client> _clients;

        public void Send(PacketType data)
        {

        }

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

            client._writer.WriteLine("Connection Made. \n Please Enter a Nickname");
            client._writer.Flush();

            client.nickName = client._reader.ReadLine();
            Console.WriteLine(client.nickName + " assigned");

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

        //private string GetReturnMessage(string code)
        //{
        //    switch (code)
        //    {
        //        case "1":
        //           return "Message reply for choice 1";
        //        case "2":
        //            return "Message reply for choice 2";
        //        case "3":
        //            return "END";
        //        default:
        //            return "Choose 1 or 2";
        //    }
        //}
    }
}
