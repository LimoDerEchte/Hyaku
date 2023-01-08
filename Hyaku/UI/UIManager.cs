
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading;
using Atto;
using HarmonyLib;
using Hyaku.GameManagement;
using Hyaku.Networking;
using Hyaku.Networking.Packets.Bidirectional;
using MelonLoader;
using UnityEngine;
using UnityEngine.UI;
using Input = UnityEngine.Input;
using Object = UnityEngine.Object;

namespace Hyaku.UI
{
    public class UIManager
    {
        public static GameObject Canvas;
        public static GameObject ConnectUI;
        public static GameObject ConnectButton;
        public static GameObject Chat;
        public static GameObject TitleScreen;
        public static GameObject TitleBack;
        public static GameObject SingleplayerButton;
        public static GameObject LobbyMpButton;
        public static GameObject MpButton;
        public static GameObject ExitButton;
        public static GameObject TitleDirector;
        public static GameObject MainLayout;
        public static GameObject LobbyScreen;
        public static GameObject LobbyLoading;
        public static List<GameObject> LobbyElementList;
        public static Transform LobbyElementListContentHolder;
        public static InputField IPInput;
        public static InputField UsernameInput;
        public static InputField PasswordInput;
        public static InputField LobbyNameInput;
        public static InputField LobbyUsernameInput;
        public static InputField LobbyPasswordInput;
        public static Text Disconnected;
        public static Text LobbyErrorMessage;
        public static Text VersionText;
        public static string ErrorMessage = "";
        public static readonly List<string> ChatMessages = new List<string>();
        public static Text ChatContent;
        public static InputField ChatInput;

        public static void InitUI()
        {
            try
            {
                if (Canvas != null && Canvas.transform != null)
                {
                    VersionText.color = Color.white;
                    return;
                }
                Canvas = AssetManager.UI.LoadAsset<GameObject>("HyakuCanvas");
                Canvas = Object.Instantiate(Canvas);
                MainLayout = GameObject.Find("MainLayout");
                TitleDirector = GameObject.Find("TitleDirector");
                ConnectButton = GameObject.Find("Connect");
                IPInput = GameObject.Find("IPInput").GetComponent<InputField>();
                UsernameInput = GameObject.Find("UsernameInput").GetComponent<InputField>();
                PasswordInput = GameObject.Find("PasswordInput").GetComponent<InputField>();
                Disconnected = GameObject.Find("Disconnected").GetComponent<Text>();
                VersionText = GameObject.Find("ModVersion").GetComponent<Text>();
                ConnectUI = GameObject.Find("ConnectUI");
                TitleScreen = GameObject.Find("TitleScreen");
                SingleplayerButton = GameObject.Find("Singleplayer");
                LobbyMpButton = GameObject.Find("LobbyMultiplayer");
                MpButton = GameObject.Find("Multiplayer");
                ExitButton = GameObject.Find("ExitButton");
                Chat = GameObject.Find("Chat");
                ChatContent = GameObject.Find("ChatContent").GetComponent<Text>();
                ChatInput = GameObject.Find("ChatInput").GetComponent<InputField>();
                TitleBack = GameObject.Find("TitleBack");
                LobbyScreen = GameObject.Find("Lobbies");
                LobbyElementListContentHolder = GameObject.Find("LobbyListContent").transform;
                LobbyUsernameInput = GameObject.Find("LobbyUsernameInput").GetComponent<InputField>();
                LobbyPasswordInput = GameObject.Find("LobbyPasswordInput").GetComponent<InputField>();
                LobbyNameInput = GameObject.Find("LobbyIDInput").GetComponent<InputField>();
                LobbyErrorMessage = GameObject.Find("LobbyErrorMessage").GetComponent<Text>();
                LobbyLoading = GameObject.Find("LobbyLoading");
                GameObject.Find("ConnectBack").GetComponent<Button>().onClick.AddListener(OnBackClick);
                GameObject.Find("LobbyBack").GetComponent<Button>().onClick.AddListener(OnBackClick);
                GameObject.Find("JoinLobbyID").GetComponent<Button>().onClick.AddListener(OnLobbyJoinClicked);
                GameObject.Find("CreateLobbyButton").GetComponent<Button>().onClick.AddListener(OnLobbyJoinClicked);
            
                ChatContent.text = String.Join("\n", ChatMessages);
                VersionText.text = VersionText.text.Replace("{version}", BuildInfo.Version);
                LobbyErrorMessage.text = "";
                Disconnected.text = "";
                LobbyScreen.SetActive(false);
                TitleBack.SetActive(false);
                ChatInput.gameObject.SetActive(false);
                ConnectUI.SetActive(false);
                TitleScreen.SetActive(false);
                Chat.SetActive(false);
                TitleDirector.SetActive(false);
                MainLayout.SetActive(false);
            
                TitleBack.GetComponent<Button>().onClick.AddListener(OnBackClick);
                ConnectButton.GetComponent<Button>().onClick.AddListener(OnConnectClicked);
                SingleplayerButton.GetComponent<Button>().onClick.AddListener(OnSingleplayerClicked);
                LobbyMpButton.GetComponent<Button>().onClick.AddListener(OnLobbyMPClick);
                MpButton.GetComponent<Button>().onClick.AddListener(OnMPClick);
                ExitButton.GetComponent<Button>().onClick.AddListener(OnExitClick);
                SetErrorMessage(ErrorMessage);
            }
            catch (Exception)
            {
                MelonLogger.Warning("Error in InitUI");
            }
        }

        public static void CreateLobbyListObject(string name, int users, int i)
        {
            GameObject listObj = Object.Instantiate(AssetManager.UI.LoadAsset<GameObject>("LobbyListElement"), LobbyElementListContentHolder);
            listObj.transform.Find("LobbyID").GetComponent<Text>().text = name;
            listObj.transform.Find("LobbyCount").GetComponent<Text>().text = $"{users} Users";
            listObj.GetComponent<RectTransform>().anchoredPosition -= new Vector2(0, i * 55);
            LobbyElementList.Add(listObj);
        }

        public static void UpdateLobbyList(Dictionary<string, int> lobbies)
        {
            if(LobbyElementListContentHolder == null)
                return;
            
            if (LobbyElementList != null && LobbyElementList.Count > 0)
                LobbyElementList.ForEach(Object.Destroy);
            else
                LobbyElementList = new List<GameObject>();

            if (lobbies.Count <= 0)
            {
                LobbyLoading.GetComponent<Text>().text = "Seems like no one's playing 3:";
                LobbyLoading.SetActive(true);
                return;
            }
            
            int i = 0;
            foreach(KeyValuePair<string, int> lobby in lobbies)
            {
                CreateLobbyListObject(lobby.Key, lobby.Value, i);
                i++;
            }
            LobbyElementListContentHolder.GetComponent<RectTransform>().sizeDelta.Set(0, Math.Max(50 + i * 55, 435));
            LobbyLoading.SetActive(false);
        }

        private static void SendListRefreshRequest()
        {
            try
            {
                WebRequest req = WebRequest.CreateDefault(new Uri($"http://{Hyaku.DefaultIP}:{Hyaku.DefaultQueryPort}/"));
                req.Method = "GET";
                WebResponse res = req.GetResponse();
                string text = new StreamReader(res.GetResponseStream() ?? throw new InvalidOperationException()).ReadToEnd();
                string[] splits = text.Split('|');
                int amount = Int32.Parse(splits[0]);
                Dictionary<string, int> lobbies = new Dictionary<string, int>();
                for (int i = 0; i < amount; i++)
                {
                    lobbies.Add(splits[i * 2 + 1], Int32.Parse(splits[i * 2 + 2]));
                }
                UpdateLobbyList(lobbies);
            }
            catch (Exception e)
            {
                MelonLogger.Warning(e);
                throw;
            }
        }

        public static void OpenConnectUI(string defaultIP)
        {
            VersionText.color = Color.black;
            IPInput.text = defaultIP;
            TitleScreen.SetActive(true);
        }
        
        public static void CloseConnectUI()
        {
            ConnectUI.SetActive(false);
        }

        public static void OpenChat()
        {
            Chat.SetActive(true);
        }

        public static void ResetChat()
        {
            ChatMessages.Clear();
            ChatContent.text = "";
        }

        public static void AddChatMessage(string message)
        {
            ChatMessages.Add(message);
            ChatContent.text = String.Join("\n", ChatMessages);
        }

        public static void OnSingleplayerClicked()
        {
            TitleScreen.SetActive(false);
            MainLayout.SetActive(true);
            TitleDirector.SetActive(true);
            Traverse.Create(global::TitleDirector.instance).Field<StateMachine<TitleStates>>("titleStateMachine").Value.CurrentState = TitleStates.SelectSaveFile;
            TitleBack.SetActive(true);
        }

        public static void OnLobbyMPClick()
        {
            TitleScreen.SetActive(false);
            LobbyScreen.SetActive(true);
        }

        public static void OnMPClick()
        {
            TitleScreen.SetActive(false);
            ConnectUI.SetActive(true);
        }
        
        public static void OnBackClick()
        {
            TitleScreen.SetActive(true);
            ConnectUI.SetActive(false);
            LobbyScreen.SetActive(false);
            TitleDirector.SetActive(false);
            MainLayout.SetActive(false);
            TitleBack.SetActive(false);
        }

        public static void OnExitClick()
        {
            Application.Quit();
        }

        public static void OnLobbyJoinClicked()
        {
            if (LobbyUsernameInput.text.Length < 3 || LobbyUsernameInput.text.Length > 16)
            {
                SetErrorMessage("Your username has to have between 3 and 16 characters!");
                return;
            }
            if (!Client.NameRegex.IsMatch(LobbyUsernameInput.text))
            {
                SetErrorMessage("Your username can only contain a-z A-Z 0-9 _ and - !");
                return;
            }
            if (LobbyNameInput.text.Length < 3 || LobbyNameInput.text.Length > 16)
            {
                SetErrorMessage("The lobby's name has to have between 3 and 16 characters!");
                return;
            }
            if (!Client.NameRegex.IsMatch(LobbyNameInput.text))
            {
                SetErrorMessage("The lobby's name can only contain a-z A-Z 0-9 _ and - !");
                return;
            }
            Client.instance.ip = Hyaku.DefaultIP;
            Client.instance.port = Hyaku.DefaultPort;
            Client.instance.username = LobbyUsernameInput.text;
            Client.instance.password = LobbyPasswordInput.text;
            Client.instance.lobby = LobbyNameInput.text;
            CloseConnectUI();
            Client.instance.ConnectToServer();
        }

        private static int _refreshCountdown = 100;
        public static void FixedUpdate()
        {
            _refreshCountdown--;
            if (_refreshCountdown <= 0)
            {
                _refreshCountdown = 300;
                new Thread(SendListRefreshRequest).Start();
            }
        }
        
        public static void OnConnectClicked()
        {
            if (IPInput != null && UsernameInput != null)
            {
                if (!IPInput.text.Equals("") &&
                    !UsernameInput.text.Equals(""))
                {
                    if (UsernameInput.text.Length < 3 || UsernameInput.text.Length > 16)
                    {
                        SetErrorMessage("Your Username has to have between 3 and 16 characters!");
                        return;
                    }
                    if (!Client.NameRegex.IsMatch(UsernameInput.text))
                    {
                        
                        SetErrorMessage("Your Username can only contain a-z A-Z 0-9 _ and - !");
                        return;
                    }
                    string[] splitIP = IPInput.text.Split(':');
                    string username = UsernameInput.text;
                    if (splitIP.Length == 1 || splitIP.Length == 2)
                    {
                        Client.instance.username = username;
                        Client.instance.ip = splitIP[0];
                        if (splitIP.Length == 2)
                        {
                            if (int.TryParse(splitIP[1], out int port))
                            {
                                Client.instance.port = port;
                            }else
                                SetErrorMessage("Invalid Port Format -> using default port");
                        }

                        Client.instance.password = PasswordInput.text;
                        CloseConnectUI();
                        Client.instance.ConnectToServer();
                    }else
                        SetErrorMessage("Invalid IP Format!");
                }else
                    SetErrorMessage("Invalid IP or Username!");
            }else
                MelonLogger.Warning("Connect Button clicked but Input Objects are missing!");
        }

        public static void SetErrorMessage(string Message)
        {
            ErrorMessage = Message;
            if (Disconnected != null)
            {
                Disconnected.text = Message;
                LobbyErrorMessage.text = Message;
            }
        }

        public static void Update()
        {
            if(Chat == null || !Chat.activeSelf || Math.Abs(Time.timeScale - 1F) > 0.2F || !ChatInput.isFocused)
                return;
            if (Input.GetKeyDown(KeyCode.Return))
            {
                if (ChatInput.text.Trim() != "")
                {
                    new ChatMessageC2S(ChatInput.text).Send();
                    ChatInput.text = "";
                }
            }
        }
    }
}