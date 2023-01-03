using System;
using MelonLoader;

namespace Hyaku.Networking.Packets.Bidirectional
{
    
    public class EndingCompletionPacketC2S : ClientToServerPacket
    {
        public string Ending;
        
        public EndingCompletionPacketC2S() : base(10) { }
        public EndingCompletionPacketC2S(string ending) : base(10)
        {
            Ending = ending;
        }
        
        public override void Send()
        {
            Packet.Write(Ending);
            PacketHandler.SendTcpData(Packet);
        }
    }
    
    public class EndingCompletionPacketS2C : ServerToClientPacket
    {
        public EndingCompletionPacketS2C() : base(10) { }

        public override void handle(Packet packet)
        {
            if (Enum.TryParse(packet.ReadString(), out EndingTypes ending))
            {
                GameManagement.ProgressionUtils.AddEnding(ending);
            }else
                MelonLogger.Warning("Server send a non existing ending!");
        }
    }
}