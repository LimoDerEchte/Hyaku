using System;

namespace Hyaku.Networking.Packets.ClientToServer
{
    public class WelcomePacket : ClientToServerPacket
    {
        public String Username;
        public String Password;
        public int MyID;
        
        public WelcomePacket() : base(999) {}
        public WelcomePacket(String username, String password, int myID) : base(999)
        {
            Username = username;
            Password = password;
            MyID = myID;
        }

        public override void Send()
        {
            Packet.Write(MyID);
            Packet.Write(Username);
            Packet.Write(Password);
            Packet.Write(BuildInfo.Version);
            PacketHandler.SendTcpData(Packet);
        }
    }
}