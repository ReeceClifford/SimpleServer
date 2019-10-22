using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.IO;

namespace SimpleClient
{

    class SimpleClient
    {
        TcpClient tcpClient;
        NetworkStream stream;
        StreamWriter writer;
        StreamReader reader;
        bool closed;

        ClientForm messageForm;


        public void SimpleClientMain()
        {
            tcpClient = new TcpClient();
            messageForm = new ClientForm();
        }

        public bool Connect(string ipAddress, int port)
        {
            try
            {
                tcpClient.Connect(ipAddress, port);
                stream = tcpClient.GetStream();
                reader = new StreamReader(stream, System.Text.Encoding.UTF8);
                writer = new StreamWriter(stream, System.Text.Encoding.UTF8);

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
            string userInput;
            //ProcessServerResponse();
            Thread t = new Thread(Listen);
            t.Start();

            while ((userInput = Console.ReadLine()) != null) 
            {
                writer.WriteLine(userInput);
                writer.Flush();

               // ProcessServerResponse();

                if (userInput == "3")
                    break;
                
            };
            closed = true;
            tcpClient.Close();
        }

        private void ProcessServerResponse()
        {
            Console.WriteLine("Server says: " + reader.ReadLine());
            Console.WriteLine();
        }

        private void Listen()
        {
            while(!closed)
            {
                string recievedMessage = reader.ReadLine();
                if(recievedMessage != null)
                {
                    Console.WriteLine(recievedMessage);
                }
            }
        }
    }
}
