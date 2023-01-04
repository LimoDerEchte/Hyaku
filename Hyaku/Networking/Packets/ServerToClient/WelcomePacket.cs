using System.Net;
using Hyaku.Utility;
using MelonLoader;

namespace Hyaku.Networking.Packets.ServerToClient
{
    public class WelcomePacket : ServerToClientPacket
    {
        public WelcomePacket() : base(999)
        {
        }

        public override void handle(Packet packet)
        {
            int myID = packet.ReadInt();
            Client.instance.myId = myID;
            MelonLogger.Msg("Got Information from Server - Sending own Information");
            new ClientToServer.WelcomePacket(Client.instance.username, Hashing.GetHashString(Client.instance.password), Client.instance.lobby, myID).Send();
        }
    }
}