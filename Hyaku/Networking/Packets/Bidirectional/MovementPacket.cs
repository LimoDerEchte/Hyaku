
using Hyaku.GameManagement;
using UnityEngine;

namespace Hyaku.Networking.Packets.Bidirectional
{
    public class MovementPacketC2S : ClientToServerPacket
    {
        public Vector3 Position;
        
        public MovementPacketC2S() : base(1) { }
        public MovementPacketC2S(Vector3 position) : base(1)
        {
            Position = position;
        }
        
        public override void Send()
        {
            Packet.Write(Position);
            PacketHandler.SendTcpData(Packet);
        }
    }
    
    public class MovementPacketS2C : ServerToClientPacket
    {
        public Vector3 Position;
        public int OwnerID;
        
        public MovementPacketS2C() : base(1) { }

        public override void handle(Packet packet)
        {
            OwnerID = packet.ReadInt();
            Position = packet.ReadVector3();
            if (PlayerManager.Instance.Heroes.ContainsKey(OwnerID))
            {
                PlayerManager.Instance.Heroes[OwnerID].SetDesiredPos(Position);
            }
        }
    }
}