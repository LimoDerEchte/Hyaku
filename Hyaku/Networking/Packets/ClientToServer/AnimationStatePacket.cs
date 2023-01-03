using System.Reflection;
using UnityEngine;
using static HarmonyLib.AccessTools;

namespace Hyaku.Networking.Packets.ClientToServer
{
    public class AnimationStatePacket : ClientToServerPacket
    {
        private static readonly FieldInfo AnimationState = Field(typeof(Hero), "currentAnimationState");
        private static int _lastAnimationState;
        private static bool _lastRotationState;
        
        public AnimationStatePacket() : base(2) { }
        
        public override void Send()
        {
            Hero hero = GameObject.Find("Hero").GetComponent<Hero>();
            int state = (int) AnimationState.GetValue(hero);
            bool rotation = hero.transform.localScale.x > 0;
            if(state == _lastAnimationState && rotation == _lastRotationState)
                return;
            _lastAnimationState = state;
            _lastRotationState = rotation;
            Packet.Write(state);
            Packet.Write(rotation);
            PacketHandler.SendTcpData(Packet);
        }

        public void ForceSend()
        {
            Hero hero = GameObject.Find("Hero").GetComponent<Hero>();
            int state = (int) AnimationState.GetValue(hero);
            bool rotation = hero.transform.localScale.x > 0;
            _lastAnimationState = state;
            _lastRotationState = rotation;
            Packet.Write(state);
            Packet.Write(rotation);
            PacketHandler.SendTcpData(Packet);
        }
    }
}