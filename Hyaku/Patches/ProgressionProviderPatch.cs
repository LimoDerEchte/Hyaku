using HarmonyLib;
using Hyaku.Networking.Packets.Bidirectional;

namespace Hyaku.Patches
{
    [HarmonyPatch(typeof(ProgressionProvider), nameof(ProgressionProvider.CollectHint))]
    public class ProgressionProviderPatch
    {
        static void Postfix(int hintId)
        {
            if (!Hyaku.CollectingEnabled) return;
            new HintCollectionPacketC2S(((EndingTypes) hintId).ToString()).Send();
        }
    }
}