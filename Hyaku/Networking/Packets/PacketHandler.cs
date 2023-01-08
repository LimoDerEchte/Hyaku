using System;
using System.Collections.Generic;
using Hyaku.Utility;
using MelonLoader;
using UnityEngine;

namespace Hyaku.Networking.Packets
{
    public class PacketHandler : MonoBehaviour
    {
        public static PacketHandler Instance;
        public Dictionary<int, ClientToServerPacket> ClientBound;
        public Dictionary<int, ServerToClientPacket> ServerBound;

        public void Init()
        {
            ClientBound = new Dictionary<int, ClientToServerPacket>();
            foreach (ClientToServerPacket packet in ReflectiveEnumerator.GetEnumerableOfType<ClientToServerPacket>())
            {
                ClientBound.Add(packet.ID, packet);
            }
            ServerBound = new Dictionary<int, ServerToClientPacket>();
            foreach (ServerToClientPacket packet in ReflectiveEnumerator.GetEnumerableOfType<ServerToClientPacket>())
            {
                ServerBound.Add(packet.ID, packet);
            }
            MelonLogger.Msg($"Loaded {ClientBound.Count} Client to Server Packets and {ServerBound.Count} Server to Client Packets");
        }
        
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                Init();
            }
            else if (Instance != this)
            {
                MelonLogger.Warning("Instance already exists, destroying object! (PacketHandler.cs)");
                Destroy(this);
            }
        }

        public void Handle(Packet packet)
        {
            try
            {
                ServerBound[packet.ReadInt()].handle(packet);
            }
            catch (Exception e)
            {
                MelonLogger.Warning($"Error while handling {packet.GetType().Name}");
            }
        }
        
        public static void SendTcpData(Packet packet)
        {
            packet.WriteLength();
            if(Client.instance != null && Client.instance.tcp != null && Client.instance.tcp.socket != null)
                Client.instance.tcp.SendData(packet);
        }
    }
}