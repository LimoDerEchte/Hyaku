using System;

namespace Hyaku.Networking.Packets.ClientToServer
{
    public class WelcomePacket : ClientToServerPacket
    {
        public string Username;
        public string Password;
        public string Lobby;
        public int MyID;
        
        public WelcomePacket() : base(999) {}
        public WelcomePacket(string username, string password, string lobby, int myID) : base(999)
        {
            Username = username;
            Password = password;
            Lobby = lobby;
            MyID = myID;
        }

        public override void Send()
        {
            Packet.Write(MyID);
            Packet.Write(BuildInfo.Version);
            Packet.Write(Username);
            Packet.Write(Password);
            Packet.Write(Lobby);
            PacketHandler.SendTcpData(Packet);
        }
    }
}