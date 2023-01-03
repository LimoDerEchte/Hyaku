using Hyaku.UI;

namespace Hyaku.Networking.Packets.Bidirectional
{
    public class ChatMessageC2S : ClientToServerPacket
    {
        public string Message;
        
        public ChatMessageC2S() : base(14) { }
        public ChatMessageC2S(string message) : base(14)
        {
            Message = message;
        }
        
        public override void Send()
        {
            Packet.Write(Message);
            PacketHandler.SendTcpData(Packet);
        }
    }

    public class ChatMessageS2C : ServerToClientPacket
    {
        public ChatMessageS2C() : base(14) { }

        public override void handle(Packet packet)
        {
            UIManager.AddChatMessage(packet.ReadString());
        }
    }
}