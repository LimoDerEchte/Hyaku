
using System;
using Hyaku.Networking;
using Hyaku.Networking.Packets.Bidirectional;
using Hyaku.Networking.Packets.ClientToServer;
using Hyaku.UI;
using Hyaku.Utility;
using UnityEngine;

namespace Hyaku.GameManagement
{
    public static class GameLogic
    {
        private static Vector3 _lastPosition;
        private static int _packetCooldown;

        public static int KickCountdown = -1;
        
        public static void Update()
        {
            ThreadManager.Update();
        }
        
        public static void FixedUpdate()
        {
            if (KickCountdown > 0)
            {
                KickCountdown--;
                if (KickCountdown <= 0)
                {
                    KickCountdown = -1;
                    UIManager.ErrorMessage = "Timed out";
                    Client.instance.tcp.socket.Close();
                    Client.instance.tcp.socket = null;
                    UIManager.openConnectUI(Client.instance.ip);
                }
            }
            
            var hero = Hero.instance;
            _packetCooldown--;
            if (hero == null || _packetCooldown > 0) return;
            _packetCooldown = 3;
            var currentPosition = hero.transform.position;
            if (Math.Abs(currentPosition.x - _lastPosition.x) > 0.1F || Math.Abs(currentPosition.y - _lastPosition.y) > 0.05F)
            {
                new MovementPacketC2S(currentPosition).Send();
                _lastPosition = currentPosition;
            }
            new AnimationStatePacket().Send();
        }
    }
}