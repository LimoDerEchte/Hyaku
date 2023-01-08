
using System;
using System.Collections.Generic;
using Atto;
using Hyaku.GameManagement;
using Hyaku.Networking;
using Hyaku.Networking.Packets;
using Hyaku.Networking.Packets.Bidirectional;
using Hyaku.UI;
using MelonLoader;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Hyaku
{
    public static class BuildInfo
    {
        public const string Name = "Hyaku";
        public const string Description = "Hyaku is a mod that tries to add online multiplayer support to the game Reventure"; 
        public const string Author = "LimoDerEchte"; 
        public const string Company = "Lime"; 
        public const string Version = "0.2.0"; 
        public const string DownloadLink = "https://www.nexusmods.com/reventure/mods/1"; 
    }
    
    public class Hyaku : MelonMod
    {
        public static bool CollectingEnabled = true;
        public static event Action GameplaySceneLoaded;
        public static String LastSceneInit;
        private static bool _skinUpdateNextFrame;

        public static readonly string DefaultIP = false ? "127.0.0.1" : "202.61.198.7";
        public static readonly int DefaultPort = 26950;
        public static readonly int DefaultQueryPort = 26951;

        public override void OnApplicationStart()
        {
            AssetManager.Init();
            MelonLogger.Msg("Mod initialized!");
        }

        public override void OnSceneWasInitialized(int buildIndex, string sceneName)
        {
            if (sceneName == "2.Title")
            {
                UIManager.InitUI();
                if(Client.instance == null)
                    LoadClasses();
                if(Client.instance.tcp != null)
                    Client.instance.Disconnect();
                PlayerManager.Instance.Reset();
                UIManager.OpenConnectUI("");
            }

            if (Client.instance == null || Client.instance.tcp == null || Client.instance.tcp.socket == null)
            {
                LastSceneInit = sceneName;
                return;
            }

            if(sceneName != LastSceneInit && sceneName != "2.Title")
                UIManager.InitUI();
            if (sceneName == "5.Gameplay")
            {
                if (LastSceneInit != "5.Gameplay")
                {
                    OnlineHero.InitScene();
                    if (GameplaySceneLoaded != null) GameplaySceneLoaded.Invoke();
                    UIManager.OpenChat();
                    new SkinUpdatePacketC2S().Send();
                    Core.Get<IHeroSkinsService>().onSpritesChange += sprites =>
                    {
                        if (Client.instance != null && Client.instance.tcp != null)
                            _skinUpdateNextFrame = true;
                    };
                }
            }
            if (sceneName == "6.Ending")
            {
                if (EndingDirector.currentLoadedEnding != null && CollectingEnabled)
                {
                    new EndingCompletionPacketC2S(EndingDirector.currentLoadedEnding.ToString()).Send();
                }
            }
            LastSceneInit = sceneName;
        }

        public override void OnUpdate()
        {
            GameLogic.Update();
            //UIManager.Update();
            if (_skinUpdateNextFrame)
            {
                new SkinUpdatePacketC2S().Send();
                _skinUpdateNextFrame = false;
            }
        }

        public override void OnFixedUpdate()
        {
            GameLogic.FixedUpdate();
            UIManager.FixedUpdate();
        }

        private void LoadClasses()
        {
            GameObject hyaku = new GameObject("HyakuScriptHolder");
            Object.DontDestroyOnLoad(hyaku);
            hyaku.AddComponent<PacketHandler>();
            hyaku.AddComponent<PlayerManager>();
            hyaku.AddComponent<Client>();
        }
    }
}