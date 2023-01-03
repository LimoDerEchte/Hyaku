using Hyaku.GameManagement;

namespace Hyaku.Networking.Packets.ServerToClient
{
    public class DespawnPlayerPacket : ServerToClientPacket
    {
        public DespawnPlayerPacket() : base(101)
        {
        }

        public override void handle(Packet packet)
        {
            PlayerManager.Instance.DeSpawnPlayer(packet.ReadInt());
        }
    }
}