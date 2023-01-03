using Hyaku.GameManagement;
using Hyaku.Utility;
using MelonLoader;
using UnityEngine;
using UniverseLib.Runtime;

namespace Hyaku.Networking.Packets.Bidirectional
{
    public class SkinUpdatePacketC2S : ClientToServerPacket
    {
        public SkinUpdatePacketC2S() : base(3) { }
        
        public override void Send()
        {
            if (GraphicsUtil.TryGetPlayerTexture(out Texture2D tex))
            {
                Packet.Write(tex);
                PacketHandler.SendTcpData(Packet);
                //MelonLogger.Msg($"SENT TEXTURE :: {tex.width} :: {tex.height}");
            }else
                MelonLogger.Warning("Couldn't retrieve player's texture for syncing!");
        }
    }
    
    public class SkinUpdatePacketS2C : ServerToClientPacket
    {
        public SkinUpdatePacketS2C() : base(3) { }
        
        public override void handle(Packet packet)
        {
            int client = packet.ReadInt();
            Texture2D tex = packet.ReadTexture2D();
            //MelonLogger.Msg($"RECEIVED TEXTURE :: {client} :: {tex.width} :: {tex.height}");
            if (PlayerManager.Instance.Info.ContainsKey(client))
            {
                GraphicsUtil.WriteToSkinDict(PlayerManager.Instance.Info[client], tex);
            }
        }
    }
}