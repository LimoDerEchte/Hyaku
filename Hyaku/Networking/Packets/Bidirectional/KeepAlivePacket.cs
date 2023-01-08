namespace Hyaku.Networking.Packets.Bidirectional
{
    
    public class KeepAlivePacketC2S : ClientToServerPacket
    {
        public KeepAlivePacketC2S() : base(-1) { }
        
        public override void Send()
        {
            PacketHandler.SendTcpData(Packet);
        }
    }
    
    public class KeepAlivePacketS2C : ServerToClientPacket
    {
        public KeepAlivePacketS2C() : base(-1) { }

        public override void handle(Packet packet)
        {
            new KeepAlivePacketC2S().Send();
        }
    }
}