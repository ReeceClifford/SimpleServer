using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

public enum PacketType
{
    EMPTY,
    CHATMESSAGE,
    NICKNAME,
    LOGIN,
    CLIENTSCONNECTED,
    DISCONNECT,
    CLEARCONNECTEDLIST,
    DISCONNECTEDCLIENT,
}

[Serializable]
public class Packet
{
    public PacketType type = PacketType.EMPTY;
}

[Serializable]
public class ChatMessagePacket : Packet
{
    public string message = String.Empty;
    public ChatMessagePacket(string message)
    {
        this.type = PacketType.CHATMESSAGE;
        this.message = message;
    }
}

[Serializable]
public class ConnectedNicknames : Packet
{
    public string nicknamesConnected;
    public ConnectedNicknames(string nicknamesConnected)
    {
        this.type = PacketType.CLIENTSCONNECTED;
        this.nicknamesConnected = nicknamesConnected;
    }
}

[Serializable]
public class NickNamePacket : Packet
{
    public string nickName;
    public NickNamePacket(string nickName)
    {
        this.type = PacketType.NICKNAME;
        this.nickName = nickName;
    }
}

//public class DisconnectedNicknames : Packet
//{
//    public string disconnectedNickname;
//    public DisconnectedNicknames(string disconnectedNickname)
//    {
//        this.type = PacketType.CLIENTSCONNECTED;
//        this.disconnectedNickname = disconnectedNickname;
//    }
//}

[Serializable]
public class DisconnectPacket : Packet
{
    public bool clientDisconnect;
    public DisconnectPacket(bool clientDiscconnect)
    {
        this.type = PacketType.DISCONNECT;
        this.clientDisconnect = true;
    }
}

//Added for UDP and TCP Task
[Serializable]
public class LoginPacket : Packet
{
    public EndPoint endPoint;
    public LoginPacket(EndPoint endPoint)
    {
        this.type = PacketType.LOGIN;
        this.endPoint = endPoint;
    }
}


