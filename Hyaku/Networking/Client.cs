using System;
using System.Net;
using System.Net.Sockets;
using Hyaku.GameManagement;
using Hyaku.Networking.Packets;
using Hyaku.UI;
using Hyaku.Utility;
using MelonLoader;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;

namespace Hyaku.Networking
{
    public class Client : MonoBehaviour
    {
        public static Client instance;
        public static int dataBufferSize = 4096;

        public string ip = "127.0.0.1";
        public int port = 26950;
        public int myId = 0;
        public string username = "InvalidUName";
        public string password = "";
        public TCP tcp;

        private bool isConnected = false;

        public void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else if (instance != this)
            {
                MelonLogger.Warning("Instance already exists, destroying object! (Client.cs)");
                Destroy(this);
            }
        }

        private void OnApplicationQuit()
        {
            Disconnect(); 
        }

        public void ConnectToServer()
        {
            MelonLogger.Msg("Connecting to server...");
            tcp = new TCP();

            isConnected = true;
            tcp.Connect(); 
        }

        public class TCP
        {
            public TcpClient socket;

            private NetworkStream stream;
            private Packet receivedData;
            private byte[] receiveBuffer;

            public void Connect()
            {
                socket = new TcpClient
                {
                    ReceiveBufferSize = dataBufferSize,
                    SendBufferSize = dataBufferSize
                };
                receiveBuffer = new byte[dataBufferSize];
                GameLogic.KickCountdown = 600;
                socket.BeginConnect(instance.ip, instance.port, ConnectCallback, socket);
            }

            private void ConnectCallback(IAsyncResult _result)
            {
                socket.EndConnect(_result);
                if (!socket.Connected)
                {
                    UIManager.ErrorMessage = "Timed out";
                    UIManager.openConnectUI(instance.ip);
                    return;
                }
                GameLogic.KickCountdown = -1;
                SceneManager.LoadScene("4.Loading");
                stream = socket.GetStream();
                receivedData = new Packet();
                stream.BeginRead(receiveBuffer, 0, dataBufferSize, ReceiveCallback, null);
                MelonLogger.Msg("TCP Connection Established");
            }

            public void SendData(Packet _packet)
            {
                try
                {
                    if (socket != null)
                    {
                        stream.BeginWrite(_packet.ToArray(), 0, _packet.Length(), null, null);
                    }
                }
                catch (Exception _ex)
                {
                    MelonLogger.Warning($"Error sending data to server via TCP: {_ex}");
                }
            }

            private void ReceiveCallback(IAsyncResult _result)
            {
                try
                {
                    int _byteLength = stream.EndRead(_result);
                    if (_byteLength <= 0)
                    {
                        instance.Disconnect();
                        return;
                    }

                    byte[] _data = new byte[_byteLength];
                    Array.Copy(receiveBuffer, _data, _byteLength);

                    receivedData.Reset(HandleData(_data)); 
                    stream.BeginRead(receiveBuffer, 0, dataBufferSize, ReceiveCallback, null);
                }
                catch
                {
                    Disconnect();
                }
            }

            private bool HandleData(byte[] _data)
            {
                int _packetLength = 0;

                receivedData.SetBytes(_data);

                if (receivedData.UnreadLength() >= 4)
                {
                    _packetLength = receivedData.ReadInt();
                    if (_packetLength <= 0)
                    {
                        return true; 
                    }
                }

                while (_packetLength > 0 && _packetLength <= receivedData.UnreadLength())
                {
                    byte[] _packetBytes = receivedData.ReadBytes(_packetLength);
                    ThreadManager.ExecuteOnMainThread(() =>
                    {
                        using (Packet _packet = new Packet(_packetBytes))
                        {
                            PacketHandler.Instance.Handle(_packet);
                        }
                    });

                    _packetLength = 0; 
                    if (receivedData.UnreadLength() >= 4)
                    {
                        _packetLength = receivedData.ReadInt();
                        if (_packetLength <= 0)
                        {
                            return true; 
                        }
                    }
                }

                if (_packetLength <= 1)
                {
                    return true; 
                }

                return false;
            }

            private void Disconnect()
            {
                instance.Disconnect();

                stream = null;
                receivedData = null;
                receiveBuffer = null;
                socket = null;
            }
        }
        
        public void Disconnect()
        {
            if (isConnected)
            {
                isConnected = false;
                tcp.socket.Close();
                tcp = null;
                if (SceneManager.GetActiveScene().name != "2.Title")
                    SceneManager.LoadScene("2.Title");
                MelonLogger.Msg("Disconnected from server");
            }
        }
    }
}