using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.IO;


namespace SimpleServer
{
    class SimpleServer
    {
        TcpListener tcpListner;

        public void Server(string ipAddress, int port)
        {
            IPAddress ip = IPAddress.Parse(ipAddress);
            tcpListner = new TcpListener(ip, port);
        }

       public void Start()
        {
            tcpListner.Start();
            Console.WriteLine("Listner started.");

            Socket socket = tcpListner.AcceptSocket();
            Console.WriteLine("Connection Established");

            SocketMethod(socket);
        }

        public void Stop()
        {
            tcpListner.Stop();
        }

        private void SocketMethod(Socket socket)
        {
            String receivedMessage;
            NetworkStream stream = new NetworkStream(socket);
            StreamReader reader = new StreamReader(stream, System.Text.Encoding.UTF8);
            StreamWriter writer = new StreamWriter(stream, System.Text.Encoding.UTF8);

            writer.WriteLine("Test message for the client");
            writer.Flush();

            while ((receivedMessage = reader.ReadLine()) != null)
            {
                string returnedMessage = GetReturnMessage(receivedMessage);

                if (receivedMessage == "END")
                    break;

                writer.WriteLine(returnedMessage);
                writer.Flush();
            };
            socket.Close();
        }

        private string GetReturnMessage(string code)
        {
            switch (code)
            {
                case "1":
                   return "Message reply for choice 1";
                
                case "2":
                    return "Message reply for choice 2";
                case "3":
                    return "END";
                default:
                    return "Choose 1 or 2";
            }
        }
    }
}
