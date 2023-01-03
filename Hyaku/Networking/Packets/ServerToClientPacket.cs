namespace Hyaku.Networking.Packets
{
    public abstract class ServerToClientPacket
    {
        public int ID;
        
        public ServerToClientPacket(int id)
        {
            ID = id;
        }

        public abstract void handle(Packet packet);
    }
}