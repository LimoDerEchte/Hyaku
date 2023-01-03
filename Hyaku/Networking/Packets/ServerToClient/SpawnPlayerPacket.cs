
using Hyaku.GameManagement;
using Hyaku.Utility;
using UnityEngine;

namespace Hyaku.Networking.Packets.ServerToClient
{
    public class SpawnPlayerPacket : ServerToClientPacket
    {
        public SpawnPlayerPacket() : base(100)
        {
        }

        public override void handle(Packet packet)
        {
            Vector3 Position = packet.ReadVector3();
            string Username = packet.ReadString();
            Texture2D tex = packet.ReadTexture2D();
            int ClientID = packet.ReadInt();
            PlayerManager.Instance.SpawnPlayerPacket(Username, ClientID, Position);
            GraphicsUtil.WriteToSkinDict(PlayerManager.Instance.Info[ClientID], tex);
        }
    }
}
