using System.IO;
using MelonLoader;
using UnityEngine;

namespace Hyaku.GameManagement
{
    public static class AssetManager
    {
        public static AssetBundle UI;
        public static AssetBundle Textures;
        public static AssetBundle Animation;
        public static AssetBundle Objects;

        public static void Init()
        {
            UI = AssetBundle.LoadFromFile(Path.Combine(MelonHandler.ModsDirectory, "..", "UserData", "hyaku/ui"));
            if (UI == null) 
                MelonLogger.Warning("Failed to load AssetBundle hyaku/ui!");
            Textures = AssetBundle.LoadFromFile(Path.Combine(MelonHandler.ModsDirectory, "..", "UserData", "hyaku/texture"));
            if (Textures == null) 
                MelonLogger.Warning("Failed to load AssetBundle hyaku/texture!");
            Animation = AssetBundle.LoadFromFile(Path.Combine(MelonHandler.ModsDirectory, "..", "UserData", "hyaku/animation"));
            if (Animation == null) 
                MelonLogger.Warning("Failed to load AssetBundle hyaku/animation!");
            Objects = AssetBundle.LoadFromFile(Path.Combine(MelonHandler.ModsDirectory, "..", "UserData", "hyaku/objects"));
            if (Objects == null) 
                MelonLogger.Warning("Failed to load AssetBundle hyaku/objects!");
        }
    }
}