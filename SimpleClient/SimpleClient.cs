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

namespace SimpleClient
{

    class SimpleClient
    {
        TcpClient tcpClient;
        NetworkStream stream;
        StreamWriter writer;
        StreamReader reader;
        bool closed;

        // Windows Forms
        ClientForm messageForm;
        Thread readerThread;

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

                // Windows Forms
                readerThread = new Thread(ProcessServerResponse);
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
            string userInput;
            //ProcessServerResponse();
           Thread t = new Thread(Listen);
           t.Start();

            readerThread.Start();

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

        // Windows Forms
        public void SendMessage(string message)
        {
            writer.WriteLine(message);
            writer.Flush();
        }

        public void Stop()
        {
            readerThread.Abort();
            tcpClient.Close();
        }

        private void ProcessServerResponse()
        {
            //Console.WriteLine("Server says: " + reader.ReadLine());
            //Console.WriteLine();
            messageForm.UpdateChatWindow(reader.ReadLine());
        }

        private void Listen()
        {
            while (!closed)
            {
                string recievedMessage = reader.ReadLine();
                if (recievedMessage != null)
                {
                    Console.WriteLine(recievedMessage);
                }
            }
        }
    }
}
