
using HarmonyLib;
using MelonLoader;
using UnityEngine;

namespace Hyaku.Networking.Packets.ServerToClient
{
    public class InteractablePacket : ServerToClientPacket
    {
        public InteractablePacket() : base(12) { }

        public override void handle(Packet packet)
        {
            Vector3 pos = packet.ReadVector3();
            int Type = packet.ReadInt();
            int State = packet.ReadInt();
            switch (Type)
            {
                case 0:
                    RaycastHit2D[] hits = Physics2D.BoxCastAll(new Vector2(pos.x, pos.y), new Vector2(0.1F, 0.1F), 0, Vector2.down);
                    foreach (RaycastHit2D hit in hits)
                    {
                        if (hit.transform.TryGetComponent(out Door d))
                        {
                            if (State == 0)
                                Traverse.Create(d).Method("Close").GetValue();
                            else
                                Traverse.Create(d).Method("Open").GetValue();
                            return;
                        }
                    }
                    break;
                default:
                    MelonLogger.Warning("Received invalid Interactable Update (Type: " + Type + ")");
                    break;
            }
        }
    }
}