using HarmonyLib;
using Hyaku.Networking.Packets.ClientToServer;
using UnityEngine;

namespace Hyaku.Patches
{
    [HarmonyPatch(typeof(Door), "OnTriggerEnter2D")]
    public class DoorOpenPatch
    {
        static void Postfix(ref Door __instance, Collider2D collision)
        {
            if (!(collision.gameObject == Hero.instance.gameObject))
                return;
            new InteractablePacket(__instance.transform.position, 0, 1).Send();
        }
    }
    
    [HarmonyPatch(typeof(Door), "OnTriggerExit2D")]
    public class DoorClosePatch
    {
        static void Postfix(ref Door __instance, Collider2D collision)
        {
            if (!(collision.gameObject == Hero.instance.gameObject))
                return;
            new InteractablePacket(__instance.transform.position, 0, 0).Send();
        }
    }
}