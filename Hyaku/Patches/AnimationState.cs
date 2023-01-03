using System.Collections.Generic;
using HarmonyLib;
using MelonLoader;
using PlatformerPro;
using UnityEngine;
using AnimationState = PlatformerPro.AnimationState;

namespace Hyaku.Patches
{
    /*[HarmonyPatch(typeof(Character), nameof(Character.AnimationState), MethodType.Getter)]
    public class AnimationState
    {
        public static Dictionary<Character, PlatformerPro.AnimationState> StaticStates = new Dictionary<Character, PlatformerPro.AnimationState>();

        public static void SetStaticAnimationState(Character character, PlatformerPro.AnimationState state)
        {
            if (StaticStates.ContainsKey(character))
                StaticStates[character] = state;
            else
                StaticStates.Add(character, state);
        }

        static void Postfix(ref Character __instance, ref PlatformerPro.AnimationState __result)
        {
            if (StaticStates.ContainsKey(__instance))
                __result = StaticStates[__instance];
        }
    }

    [HarmonyPatch(typeof(Gravity), nameof(Gravity.Value), MethodType.Getter)]
    public class HeroGravity
    {
        static void Postfix(ref Gravity __instance, ref float __result)
        {
            if(__instance.transform.name != "Hero")
                __result = 0F;
        }
    }

    [HarmonyPatch(typeof(Hero), "currentAnimationState", MethodType.Setter)]
    public class AnimState
    {
        static void Postfix(ref AnimationState __result)
        {
            MelonLogger.Msg("AnimationState: " + __result);
        }
    }*/
}