using HarmonyLib;
using Hyaku.Networking;
using Hyaku.Networking.Packets.Bidirectional;

namespace Hyaku.Patches
{
    [HarmonyPatch(typeof(ProgressionProvider), nameof(ProgressionProvider.CollectHint))]
    public class ProgressionProviderPatch
    {
        static void Postfix(int hintId)
        {
            if(Client.instance == null || Client.instance.tcp == null || Client.instance.tcp.socket == null)
                return;

            if (!Hyaku.CollectingEnabled) return;
            new HintCollectionPacketC2S(((EndingTypes) hintId).ToString()).Send();
        }
    }
}