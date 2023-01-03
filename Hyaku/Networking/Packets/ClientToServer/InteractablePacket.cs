using UnityEngine;

namespace Hyaku.Networking.Packets.ClientToServer
{
    public class InteractablePacket : ClientToServerPacket
    {
        public Vector3 Pos;
        public int Type, State;

        public InteractablePacket() : base(12) { }
        public InteractablePacket(Vector3 position, int type, int state) : base(12)
        {
            Pos = position;
            Type = type;
            State = state;
        }
        
        public override void Send()
        {
            Packet.Write(Pos);
            Packet.Write(Type);
            Packet.Write(State);
            PacketHandler.SendTcpData(Packet);
        }
    }
}