using System;
using System.Collections.Generic;

namespace SWEng.Network
{

    public struct NetworkRoom
    {
        public string Name;
        public int PlayerCount;
        public int MaxPlayers;

        public NetworkRoom(string name, int playerCnt, int maxPlayer)
        {
            Name = name;
            PlayerCount = playerCnt;
            MaxPlayers = maxPlayer;
        }
    }

    public interface INetworkSystem
    {
        public INetworkRoom Room { get; }

        public bool IsMaster { get; }
        public bool IsConnect { get; }

        public string NickName { get; }
        public int CountOfPlayers { get; }
        public int CountOfLobbyPlayers { get; }

        public int PlayerId { get; }

        public Action OnConnectEvent { get; set; }

        public void Connect(string nickName);
        public void Disconnect();

        public IList<NetworkRoom> GetRoomInfor();

        public void CreateRoom(string roomName);
        public void JoinRandomRoom();
        public void JoinRoom(string name);

        public void LeaveRoom();

    }
}