using Hyaku.UI;

namespace Hyaku.Networking.Packets.ServerToClient
{
    public class KickPacket : ServerToClientPacket
    {
        public KickPacket() : base(-999) { }

        public override void handle(Packet packet)
        {
            UIManager.SetErrorMessage("Disconnected: " + packet.ReadString());
            Client.instance.Disconnect();
        }
    }
}