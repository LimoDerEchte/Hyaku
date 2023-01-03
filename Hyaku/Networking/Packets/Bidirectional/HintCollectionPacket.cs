using System;
using MelonLoader;

namespace Hyaku.Networking.Packets.Bidirectional
{
    public class HintCollectionPacketC2S : ClientToServerPacket
    {
        public string Ending;
        
        public HintCollectionPacketC2S() : base(11) { }
        public HintCollectionPacketC2S(string ending) : base(11)
        {
            Ending = ending;
        }
        
        public override void Send()
        {
            Packet.Write(Ending);
            PacketHandler.SendTcpData(Packet);
        }
    }
    
    public class HintCollectionPacketS2C : ServerToClientPacket
    {
        public HintCollectionPacketS2C() : base(11) { }

        public override void handle(Packet packet)
        {
            if (Enum.TryParse(packet.ReadString(), out EndingTypes ending))
            {
                GameManagement.ProgressionUtils.AddHint(ending);
            }else
                MelonLogger.Warning("Server send a non existing ending hint!");
        }
    }
}