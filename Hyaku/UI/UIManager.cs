
using System;
using System.Collections.Generic;
using Atto.Services.Bootstrapper;
using Hyaku.GameManagement;
using Hyaku.Networking;
using Hyaku.Networking.Packets.Bidirectional;
using MelonLoader;
using PlatformerPro;
using Steamworks;
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
        public static InputField IPInput;
        public static InputField UsernameInput;
        public static InputField PasswordInput;
        public static Text Disconnected;
        public static Text VersionText;
        public static string ErrorMessage = "";
        public static readonly List<string> ChatMessages = new List<string>();
        public static Text ChatContent;
        public static InputField ChatInput;

        public static void InitUI()
        {
            Canvas = AssetManager.UI.LoadAsset<GameObject>("HyakuCanvas");
            Canvas = Object.Instantiate(Canvas);
            Object.Destroy(GameObject.Find("MainLayout"));
            Object.Destroy(GameObject.Find("TitleDirector"));
            ConnectButton = GameObject.Find("Connect");
            IPInput = GameObject.Find("IPInput").GetComponent<InputField>();
            UsernameInput = GameObject.Find("UsernameInput").GetComponent<InputField>();
            PasswordInput = GameObject.Find("PasswordInput").GetComponent<InputField>();
            Disconnected = GameObject.Find("Disconnected").GetComponent<Text>();
            VersionText = GameObject.Find("ModVersion").GetComponent<Text>();
            ConnectUI = GameObject.Find("ConnectUI");
            Chat = GameObject.Find("Chat");
            ChatContent = GameObject.Find("ChatContent").GetComponent<Text>();
            //ChatInput = GameObject.Find("ChatInput").GetComponent<InputField>();
            ChatContent.text = String.Join("\n", ChatMessages);
            Chat.SetActive(false);
            VersionText.text = VersionText.text.Replace("{version}", BuildInfo.Version);
            ConnectButton.GetComponents<Button>()[0].onClick.AddListener(OnConnectClicked);
            SetErrorMessage(ErrorMessage);
            closeConnectUI();
            Camera
        }
        
        public static void openConnectUI(string defaultIP)
        {
            IPInput.text = defaultIP;
            ConnectUI.SetActive(true);
        }
        
        public static void closeConnectUI()
        {
            ConnectUI.SetActive(false);
        }

        public static void openChat()
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

        public static void OnConnectClicked()
        {
            if (IPInput != null && UsernameInput != null)
            {
                if (!IPInput.text.Equals("") &&
                    !UsernameInput.text.Equals(""))
                {
                    if (UsernameInput.text.Length < 3 || UsernameInput.text.Length > 20)
                    {
                        SetErrorMessage("Your Username has to be between 3 and 20 characters!");
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
                                SetErrorMessage("Invalid Port Format -> using default");
                        }

                        Client.instance.password = PasswordInput.text;
                        closeConnectUI();
                        Client.instance.ConnectToServer();
                    }else
                        SetErrorMessage("Invalid IP Format");
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
                Disconnected.GetComponent<Text>().text = Message;
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