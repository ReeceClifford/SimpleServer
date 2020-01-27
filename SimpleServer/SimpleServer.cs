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
using System.Timers;
using System.Drawing;

namespace SimpleServer
{
    class SimpleServer
    {
        List<Client> clientsList;
        List<string> clientNicknames;

        TcpListener tcpListner;

        Thread tUdpClientMethod;

        // Higher or Lower
        int hOrLNumber;
        bool hOrLActive;

        //Graphical Game Logic
        Dictionary<string, Point> clientGameTank = new Dictionary<string, Point>();
        Dictionary<string, string> tankSprite = new Dictionary<string, string>();
        Dictionary<string, Point> clientBomb = new Dictionary<string, Point>();
        Dictionary<string, string>bombSprie = new Dictionary<string, string>();

      

        int randomX;
        int randomY;

        public void Server(string ipAddress, int port)
        {
            IPAddress ip = IPAddress.Parse(ipAddress);
            tcpListner = new TcpListener(ip, port);
            clientsList = new List<Client>();
            clientNicknames = new List<string>();
        }

        public void Start()
        {
            tcpListner.Start();
            Console.WriteLine("Listener Started");
            while (true)
            {
                Socket _tcpSocket = tcpListner.AcceptSocket();
                Console.WriteLine("Connection Established");

                var client = new Client(_tcpSocket);
                clientsList.Add(client);

                Thread t = new Thread(new ParameterizedThreadStart(TCPClientMethod));
                t.Start(client);  
            };
        }

        private void TCPClientMethod(object clientObj)
        {
            Client client = (Client)clientObj;
            client.tcpConnect = true;
            try
            {
                while (client.tcpConnect)
            {
                Packet tcpPacketToHandle = client.tcpRead();
                HandlePacket(client, tcpPacketToHandle);
            }
            }
            catch (Exception e)
            {
                Console.WriteLine("TCP Read Error " + e.Message);
            }
            client.Close();
            clientsList.Remove(client);
        }

        private void UDPClientMethod(object clientObj)
        {
            Client client = (Client)clientObj;
            try
            {
                while (client.udpConnect)
                {
                    Packet udpPacketToHandle = client.udpRead();
                    Console.WriteLine("Hits udpClientMethod");
                    HandlePacket(client, udpPacketToHandle);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("UDP Read Error " + e.Message);

            }
            clientsList.Remove(client);
        }


        public void PushClientList(object clientObj)
        {
            Client client = (Client)clientObj;

            Packet connectedNicksPack = new Packet();
            connectedNicksPack = ConnectedNicknames(clientNicknames, clientObj);

            for (int i = 0; i < clientsList.Count; i++)
            {
                clientsList[i].UDPSend(connectedNicksPack);
            }
        }

        public ConnectedNicknames ConnectedNicknames(List<string> nicknames, object clientObj)
        {
            Client client = (Client)clientObj;
            ConnectedNicknames connectedNicksPack = new ConnectedNicknames(nicknames);
            connectedNicksPack.nicknamesConnected = clientNicknames;
            return connectedNicksPack;
        }

        public int HigherOrLower()
        {
            Random random = new Random();
            hOrLNumber = random.Next(1, 49);
            return hOrLNumber;
        }
        public int  MakeRandomXNumber()
        {
            Random random = new Random();
            randomX = random.Next(1, 619);
            return randomX;
        }
        public int MakeRandomYNumber()
        {
            Random random = new Random();
            randomY = random.Next(1, 379);
            return randomY;
        }
        public void PushTankGameLogic(object clientObj)
        {
            Client client = (Client)clientObj;

            GameInfoUpdate gameInfoUpdate = new GameInfoUpdate(clientGameTank);
            for (int i = 0; i < clientGameTank.Count; i++)
            {
                clientsList[i].UDPSend(gameInfoUpdate);
            }

            GameInfoSpriteUpdate gameInforSpriteUpdate = new GameInfoSpriteUpdate(tankSprite);
            for (int i = 0; i < tankSprite.Count; i++)
            {
                clientsList[i].UDPSend(gameInforSpriteUpdate);
            }

            GameInfoBombUpdate gameInfoBombUpdate = new GameInfoBombUpdate(clientBomb);
            for (int i = 0; i < clientBomb.Count; i++)
            {
                clientsList[i].UDPSend(gameInfoBombUpdate);
            }
            GameInfoBombSpriteUpdate gameInforBombSpriteUpdate = new GameInfoBombSpriteUpdate(bombSprie);
            for (int i = 0; i < bombSprie.Count; i++)
            {
                clientsList[i].UDPSend(gameInforBombSpriteUpdate);
            }


        }
        private void HandlePacket(object clientObj, Packet packet)
        {
            Client client = (Client)clientObj;
            Packet packetToHandle = packet;
            switch (packetToHandle.type)
            {
                case PacketType.CHATMESSAGE:
                    ChatMessagePacket chatPack = (ChatMessagePacket)packetToHandle;
                    Console.WriteLine(chatPack.message);
                    int recentGuess;
                   if (chatPack.message == "!HL" && !hOrLActive)
                    {
                        hOrLActive = true;
                        HigherOrLower();
                        chatPack.message = "[SERVER] HIGHER OR LOWER REQUESTED GO ON PICK HIGHER OR LOWER!";

                    }
                    else if (!Int32.TryParse(chatPack.message, out recentGuess))
                    {
                        chatPack.message = string.Format("{0:HH:mm:ss tt}", DateTime.Now) + " - [" + client.nickName + "] " + " - " + chatPack.message;
                    }
                    
                    else if (hOrLActive)
                    {
                        if (Int32.TryParse(chatPack.message, out recentGuess))
                        {
                            if (recentGuess < hOrLNumber)
                            {
                                chatPack.message = string.Format("{0:HH:mm:ss tt}", DateTime.Now) + " [SERVER] HIGHER! " + client.nickName + " guessed: " + recentGuess;
                            }
                            else if (recentGuess > hOrLNumber)
                            {
                                chatPack.message = string.Format("{0:HH:mm:ss tt}", DateTime.Now) + " [SERVER] LOWER! " + client.nickName + " guessed: " + recentGuess;
                            }
                            else if (recentGuess == hOrLNumber)
                            {
                                chatPack.message = string.Format("{0:HH:mm:ss tt}", DateTime.Now) + " [SERVER] CORRECT! THE CORRECT ANSWER WAS " + hOrLNumber;
                                hOrLActive = false;
                                hOrLNumber = 0;
                            }
                        }
                    }
                    for (int i = 0; i < clientsList.Count; i++)
                    {
                        clientsList[i].tcpSend(chatPack);
                    }
                    break;
                case PacketType.NICKNAME:
                    NickNamePacket nicknamePacket = (NickNamePacket)packetToHandle;
                    client.nickName = nicknamePacket.nickName;
                    clientNicknames.Add(client.nickName);
                    PushClientList(clientObj);
                    clientGameTank.Add(client.nickName, new Point(MakeRandomXNumber(), MakeRandomYNumber()));
                    tankSprite.Add(client.nickName, "TankSprite.png");
                    clientBomb.Add(client.nickName, new Point(MakeRandomXNumber() + 10, MakeRandomYNumber() - 40));
                    bombSprie.Add(client.nickName, "MainBombSprie.png");
                    PushTankGameLogic(clientObj);
                    break;
                case PacketType.LOGIN:
                    Console.WriteLine("Login packet Created");
                    LoginPacket loginPacket = (LoginPacket)packetToHandle;
                    client.UdpConnect(loginPacket.endPoint);
                    tUdpClientMethod = new Thread(UDPClientMethod);
                    tUdpClientMethod.Start(client);
                    break;
                case PacketType.DISCONNECT:
                    DisconnectPacket disconnectPack = (DisconnectPacket)packetToHandle;
                    Console.WriteLine("Disconnect Client " + disconnectPack.clientDisconnect);
                    if(disconnectPack.clientDisconnect)
                    {
                        clientNicknames.Remove(client.nickName);
                        clientGameTank.Remove(client.nickName);
                        tankSprite.Remove(client.nickName);
                        clientBomb.Remove(client.nickName);
                        bombSprie.Remove(client.nickName);
                    }
                    PushClientList(clientObj);
                    break;
                case PacketType.GAMEMOVE:
                    GameMovePacket gameMovePack = (GameMovePacket)packetToHandle;
                    string movementRequested = gameMovePack.gameMove;
                    GameTanksMovement(clientObj, movementRequested);
                    break;

                case PacketType.GAMEBOMBMOVE:
                    GameBombMovePacket gameBombMovePack = (GameBombMovePacket)packetToHandle;
                    string dropBomb = gameBombMovePack.gameBombMove;
                    GameBombMovement(clientObj, dropBomb);
                    break;
            }
        }
        public void GameBombMovement(object clientObj, string movementBombRequested)
        {
            Client client = (Client)clientObj;
            switch(movementBombRequested)
            {
                case "Upwards":
                    clientBomb[client.nickName] = new Point(clientBomb[client.nickName].X + 0, clientBomb[client.nickName].Y - 10);
                 

                        PushTankGameLogic(clientObj);
                    break;
                case "Downwards":

                    clientBomb[client.nickName] = new Point(clientBomb[client.nickName].X + 0, clientBomb[client.nickName].Y + 10);
                   
                    PushTankGameLogic(clientObj);
                    break;
                case "Right":
                    clientBomb[client.nickName] = new Point(clientBomb[client.nickName].X + 10, clientBomb[client.nickName].Y - 0);
                   
                    PushTankGameLogic(clientObj);
                    break;
                case "Left":
                    clientBomb[client.nickName] = new Point(clientBomb[client.nickName].X - 10, clientBomb[client.nickName].Y - 0);
                   
                    PushTankGameLogic(clientObj);
                    break;
            }
        }
        public void GameTanksMovement(object clientObj, string movementRequested)
        {
            Client client = (Client)clientObj;
            switch (movementRequested)
            {
                case "Upwards":
                    clientGameTank[client.nickName] = new Point(clientGameTank[client.nickName].X + 0, clientGameTank[client.nickName].Y - 10);
                    if (clientGameTank[client.nickName].Y < 0)
                    {
                        clientGameTank[client.nickName] = new Point(clientGameTank[client.nickName].X + 0, clientGameTank[client.nickName].Y + 10);
                        break;
                    }
                   
                    PushTankGameLogic(clientObj);
                    break;
                case "Downwards":
                    clientGameTank[client.nickName] = new Point(clientGameTank[client.nickName].X + 0, clientGameTank[client.nickName].Y + 10);
                    if (clientGameTank[client.nickName].Y > 380)
                    {
                        clientGameTank[client.nickName] = new Point(clientGameTank[client.nickName].X + 0, clientGameTank[client.nickName].Y - 10);
                        break;
                    }
                   
                    PushTankGameLogic(clientObj);
                    break;
                case "Right":
                    clientGameTank[client.nickName] = new Point(clientGameTank[client.nickName].X + 10, clientGameTank[client.nickName].Y - 0);
                    if (clientGameTank[client.nickName].X >= 620)
                    {
                        clientGameTank[client.nickName] = new Point(clientGameTank[client.nickName].X - 10, clientGameTank[client.nickName].Y - 0);
                        break;
                    }
                   
                    PushTankGameLogic(clientObj);
                    break;
                case "Left":
                    clientGameTank[client.nickName] = new Point(clientGameTank[client.nickName].X - 10, clientGameTank[client.nickName].Y - 0);
                    if (clientGameTank[client.nickName].X < 0)
                    {
                        clientGameTank[client.nickName] = new Point(clientGameTank[client.nickName].X + 10, clientGameTank[client.nickName].Y - 0);
                        break;
                    }
                 
                    PushTankGameLogic(clientObj);
                    break;
            }
        }

        public void Stop()
        {
            tcpListner.Stop();
        }
    }
}
