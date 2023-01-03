using Hyaku.GameManagement;
using UnityEngine;

namespace Hyaku.Networking.Packets.ServerToClient
{
    public class AnimationStatePacket : ServerToClientPacket
    {
        public int OwnerID;

        public AnimationStatePacket() : base(2) { }

        public override void handle(Packet packet)
        {
            OwnerID = packet.ReadInt();
            if (PlayerManager.Instance.Heroes.ContainsKey(OwnerID))
            {
                if (PlayerManager.Instance.Heroes.ContainsKey(OwnerID) &&
                    PlayerManager.Instance.Heroes[OwnerID].gameObject != null)
                {
                    OnlineHero hero = PlayerManager.Instance.Heroes[OwnerID];
                    hero.animator.CrossFade(StateToAnimName(packet.ReadInt()), 0, 0);
                    hero.animator.transform.localScale = new Vector3(packet.ReadBool() ? 1.25F : -1.25F, 1.25F, 1F);
                }
            }
        }

        public static string StateToAnimName(int state)
        {
            switch (state)
            {
                case 100:
                    return "PlayerWalk";
                case 200:
                case 210:
                    return "PlayerJumping";
                case 220:
                    return "PlayerFlying";
                case 4003:
                    return "PlayerPickup";
                case 620:
                case 600:
                    return "PlayerSwimIdle";
                case 610:
                    return "PlayerSwim";
                default:
                    return "PlayerIdle";
            }
        }
    }
}
