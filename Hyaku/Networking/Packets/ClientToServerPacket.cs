using System;

namespace Hyaku.Networking.Packets
{
    public abstract class ClientToServerPacket
    {
        public Packet Packet;
        public int ID;
        
        public ClientToServerPacket() { }
        public ClientToServerPacket(int id)
        {
            ID = id;
            Packet = new Packet(ID);
        }

        public abstract void Send();
    }
}