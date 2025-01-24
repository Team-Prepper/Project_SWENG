using System;
using System.Collections.Generic;

public struct NetworkRoom {
    public string Name;
    public int PlayerCount;
    public int MaxPlayers;

    public NetworkRoom(string name, int playerCnt, int maxPlayer) {
        Name = name;
        PlayerCount = playerCnt;
        MaxPlayers = maxPlayer;
    }
}

public interface INetwork {

    public INetworkChat Chat { get; }
    public INetworkRoom Room { get; }

    public bool IsMaster { get; }

    public string NickName { get; }
    public int CountOfPlayers { get; }
    public int CountOfLobbyPlayers { get; }

    public int PlayerId { get; }

    public void Connect(string nickName, Action connectCallback = null);
    public void Disconnect();

    public IList<NetworkRoom> GetRoomInfor();
    public void CreateRoom(string roomName);
    public void JoinRandomRoom();
    public void JoinRoom(string name);

    public void LeaveRoom();

    public void GameMasterSet();

}