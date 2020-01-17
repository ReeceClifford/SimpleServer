using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Drawing;

public enum PacketType
{
    EMPTY,
    LOGIN,
    CHATMESSAGE,
    NICKNAME,
    NICKNAMESCONNECTED,
    DISCONNECT,
    GAMEINFORMATION,
    GAMESPRITE,
    GAMEMOVE,
}

[Serializable]
public class Packet
{
    public PacketType type = PacketType.EMPTY;
}


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
    public string nickName;
    public NickNamePacket(string nickName)
    {
        this.type = PacketType.NICKNAME;
        this.nickName = nickName;
    }
}

[Serializable]
public class GameMovePacket : Packet
{
    public string gameMove;
    public GameMovePacket(string gameMove)
    {
        this.type = PacketType.GAMEMOVE;
        this.gameMove = gameMove;
    }
}

[Serializable]
public class ConnectedNicknames : Packet
{
    public List<string> nicknamesConnected;
    public ConnectedNicknames(List<string> nicknamesConnected)
    {
        this.type = PacketType.NICKNAMESCONNECTED;
        this.nicknamesConnected = nicknamesConnected;
    }
}

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

[Serializable]
public class GameInfoUpdate : Packet
{
    public Dictionary<string, Point> clientGameTankPacket;

    public GameInfoUpdate(Dictionary<string, Point> clientGameTankPacket)
    {
        this.type = PacketType.GAMEINFORMATION;
        this.clientGameTankPacket = clientGameTankPacket;
    }
}

[Serializable]
public class GameInfoSpriteUpdate : Packet
{
    public  Dictionary<string, string> cliengGameSpriteInfo;

    public GameInfoSpriteUpdate(Dictionary<string, string> cliengGameSpriteInfo)
    {
        this.type = PacketType.GAMESPRITE;
        this.cliengGameSpriteInfo = cliengGameSpriteInfo;
    }
}





