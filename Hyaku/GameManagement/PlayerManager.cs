
using System.Collections.Generic;
using MelonLoader;
using UnityEngine;

namespace Hyaku.GameManagement
{
    public class PlayerManager : MonoBehaviour
    {
        public static PlayerManager Instance;
        public Dictionary<int, PlayerInfo> Info;
        public Dictionary<int, OnlineHero> Heroes;

        public void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                Heroes = new Dictionary<int, OnlineHero>();
                Info = new Dictionary<int, PlayerInfo>();
                Hyaku.GameplaySceneLoaded += () =>
                {
                    foreach (PlayerInfo info in Info.Values)
                    {
                        if (Heroes.ContainsKey(info.ClientID))
                        {
                            if (Heroes[info.ClientID] == null || Heroes[info.ClientID].gameObject == null)
                                SpawnPlayer(info.Username, info.ClientID, Vector3.zero);
                        }
                        else
                            SpawnPlayer(info.Username, info.ClientID, Vector3.zero);
                    }
                };
            }
            else if (Instance != this)
            {
                MelonLogger.Warning("Instance already exists, destroying object! (PlayerManager.cs)");
                Destroy(this);
            }
        }

        public void SpawnPlayerPacket(string username, int clientID, Vector3 position)
        {
            if(Info.ContainsKey(clientID))
                DeSpawnPlayer(clientID);
            MelonLogger.Msg($"{username} joined the game.");
            Info.Add(clientID, new PlayerInfo(clientID, username));
            if(Hyaku.LastSceneInit == "5.Gameplay")
                SpawnPlayer(username, clientID, position);
        }

        public void SpawnPlayer(string username, int clientID, Vector3 spawnLocation)
        {
            if (!Heroes.ContainsKey(clientID))
            {
                Heroes.Add(clientID, OnlineHero.createOnlineHero(username, clientID, spawnLocation));
            }
            else
            {
                if (Heroes[clientID] != null && Heroes[clientID].gameObject != null) DestroyImmediate(Heroes[clientID].gameObject);
                Heroes[clientID] = OnlineHero.createOnlineHero(username, clientID, spawnLocation);
            }
        }

        public void DeSpawnPlayer(int clientID)
        {
            if (Info.ContainsKey(clientID))
            {
                MelonLogger.Msg($"{Info[clientID].Username} left the game.");
                if (Heroes[clientID] != null){
                    if(Heroes[clientID].gameObject != null) 
                        DestroyImmediate(Heroes[clientID].gameObject);
                    Heroes.Remove(clientID);
                }
                Info.Remove(clientID);
            }
            else
                MelonLogger.Warning($"Tried to de-spawn non existent player {clientID}!");
        }

        public void Reset()
        {
            foreach (OnlineHero hero in Heroes.Values)
            {
                if(hero != null && hero.gameObject != null)
                    DestroyImmediate(hero.gameObject);
            }
            Heroes.Clear();
            Info.Clear();
        }

        public class PlayerInfo
        {
            public readonly string Username;
            public readonly int ClientID;
            public readonly Dictionary<string, Sprite> HeroSkinDictionary;

            public PlayerInfo(int id, string username)
            {
                ClientID = id;
                Username = username;
                HeroSkinDictionary = new Dictionary<string, Sprite>();
            }
        }
    }
}