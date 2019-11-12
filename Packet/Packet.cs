using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public enum PacketType
{
    EMPTY,
    CHATMESSAGE,
    NICKNAME,
    USERLIST,
}

namespace Packet
{

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

}
