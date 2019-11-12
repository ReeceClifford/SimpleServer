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
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;

namespace SimpleClient
{
    class SimpleClient
    {
        TcpClient tcpClient;
        NetworkStream stream;
        StreamReader reader;
        StreamWriter writer;
        ClientForm messageForm;
        Thread readerThread;

        public void SimpleClientMain()
        {
            tcpClient = new TcpClient();
            messageForm = new ClientForm(this);
        }

        public bool Connect(string ipAddress, int port)
        {
            try
            {
                tcpClient.Connect(ipAddress, port);
                stream = tcpClient.GetStream();
                reader = new StreamReader(stream, System.Text.Encoding.UTF8);
                writer = new StreamWriter(stream, System.Text.Encoding.UTF8);
           
                readerThread = new Thread(Listen);

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
            readerThread.Start();
        }

        public void SendMessage(string message)
        {
            string userInput = message;
            if (userInput != "")
            {
                writer.WriteLine(userInput);
                writer.Flush();
            };
        }

        public void Stop()
        {
            readerThread.Abort();
            tcpClient.Close();
        }

        private void Listen()
        {
                string recievedMessage;
                while((recievedMessage = reader.ReadLine()) != null)
                {
                    messageForm.UpdateChatWindow(recievedMessage);
                }
            
        }
    }
}
