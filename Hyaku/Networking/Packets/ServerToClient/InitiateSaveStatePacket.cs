using System;
using Hyaku.Patches;
using MelonLoader;

namespace Hyaku.Networking.Packets.ServerToClient
{
    public class InitiateSaveStatePacket : ServerToClientPacket
    {
        public InitiateSaveStatePacket() : base(99) { }

        public override void handle(Packet packet)
        {
            int unlockedEndings = packet.ReadInt();
            for (int i = 0; i < unlockedEndings; i++)
            {
                if (Enum.TryParse(packet.ReadString(), out EndingTypes ending))
                {
                    GameManagement.ProgressionUtils.AddEnding(ending);
                }
            }
            Hyaku.CollectingEnabled = false;
            int unlockedHints = packet.ReadInt();
            for (int i = 0; i < unlockedHints; i++)
            {
                if (Enum.TryParse(packet.ReadString(), out EndingTypes ending))
                {
                    GameManagement.ProgressionUtils.AddHint(ending);
                }
            }
            Hyaku.CollectingEnabled = true;
            MelonLogger.Msg("Received " + unlockedEndings + " completed endings and " + unlockedHints + " unlocked hints.");
        }
    }
}