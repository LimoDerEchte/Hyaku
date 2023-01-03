
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

namespace Hyaku
{
    public static class BuildInfo
    {
        public const string Name = "Hyaku";
        public const string Description = "Hyaku is a mod that tries to add online multiplayer support to the game"; 
        public const string Author = "LimoDerEchte"; 
        public const string Company = "Lime"; 
        public const string Version = "0.1.3"; 
        public const string DownloadLink = "https://www.nexusmods.com/reventure/mods/1"; 
    }
    
    public class Hyaku : MelonMod
    {
        public static bool CollectingEnabled = true;
        public static event Action GameplaySceneLoaded;
        public static String LastSceneInit;
        private static bool SkinUpdateNextFrame;

        public override void OnApplicationStart()
        {
            AssetManager.Init();
            UIManager.InitUI();
            MelonLogger.Msg("Mod initialized!");
        }

        public override void OnSceneWasInitialized(int buildIndex, string sceneName)
        {
            if(sceneName != LastSceneInit)
                UIManager.InitUI();
            if (sceneName == "2.Title")
            {
                if(Client.instance == null)
                    LoadClasses();
                if(Client.instance.tcp != null)
                    Client.instance.Disconnect();
                PlayerManager.Instance.Reset();
                UIManager.openConnectUI("");
            }else 
                UIManager.closeConnectUI();
            if (sceneName == "5.Gameplay")
            {
                if (LastSceneInit != "5.Gameplay")
                {
                    OnlineHero.InitScene();
                    if (GameplaySceneLoaded != null) GameplaySceneLoaded.Invoke();
                    UIManager.openChat();
                    new SkinUpdatePacketC2S().Send();
                    Core.Get<IHeroSkinsService>().onSpritesChange += sprites =>
                    {
                        if (Client.instance != null && Client.instance.tcp != null)
                            SkinUpdateNextFrame = true;
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
            if (SkinUpdateNextFrame)
            {
                new SkinUpdatePacketC2S().Send();
                SkinUpdateNextFrame = false;
            }
        }

        public override void OnFixedUpdate()
        {
            GameLogic.FixedUpdate();
        }

        private void LoadClasses()
        {
            GameObject hyaku = new GameObject("HyakuScriptHolder");
            GameObject.DontDestroyOnLoad(hyaku);
            hyaku.AddComponent<PacketHandler>();
            hyaku.AddComponent<PlayerManager>();
            hyaku.AddComponent<Client>();
        }
    }
}