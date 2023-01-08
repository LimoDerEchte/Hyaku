using HarmonyLib;
using Hyaku.Networking;

namespace Hyaku.Patches
{
    [HarmonyPatch(typeof(SteamAchievementsProvider), "SetAchievement")]
    public class AchievmentPatch
    {
        public static void Prefix(ref EndingTypes ending)
        {
            if(Client.instance == null || Client.instance.tcp == null || Client.instance.tcp.socket == null)
                ending = EndingTypes.None;
        }
    }
}