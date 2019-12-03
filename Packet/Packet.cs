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
public class NickNamePacket : Packet
{
    public string nickName = String.Empty;
    public NickNamePacket(string nickName)
    {
        this.type = PacketType.NICKNAME;
        this.nickName = nickName;
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


