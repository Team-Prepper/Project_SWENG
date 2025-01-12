using EHTool.UIKit;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;

public class PhotonNet : MonoBehaviourPunCallbacks, INetwork {

    PhotonChat _chat;
    PhotonRoom _room;

    PhotonView _pv;

    private string gameVersion = "3";

    public int baseDiceValue = 0;

    public INetworkChat Chat => _chat;
    public INetworkRoom Room => _room;

    List<RoomInfo> _roomList = new List<RoomInfo>();

    public bool IsMaster => PhotonNetwork.IsMasterClient;

    public string NickName => PhotonNetwork.NickName;
    public int CountOfPlayers => PhotonNetwork.CountOfPlayers;
    public int CountOfLobbyPlayers => PhotonNetwork.CountOfPlayers - PhotonNetwork.CountOfPlayersInRooms;

    public int PlayerId { get; private set; }

    CallbackMethod _connectCallback;

    void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.GameVersion = gameVersion;

        _pv = gameObject.AddComponent<PhotonView>();
        _pv.ViewID = 999;

        _chat = gameObject.AddComponent<PhotonChat>();
        _room = gameObject.AddComponent<PhotonRoom>();

    }
    public void Connect(string nickName, CallbackMethod connectCallback) {
        PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.LocalPlayer.NickName = nickName;
        _connectCallback = connectCallback;
    }

    public void Disconnect()
    {
        PhotonNetwork.Disconnect();

    }

    public IList<NetworkRoom> GetRoomInfor()
    {
        IList<NetworkRoom> rooms = new List<NetworkRoom>();

        foreach (RoomInfo room in _roomList) {
            rooms.Add(new NetworkRoom(room.Name, room.PlayerCount, room.MaxPlayers));
        }
        return rooms;
    }

    public void CreateRoom(string roomName)
    {
        RoomOptions roomOptions = new RoomOptions
        {
            MaxPlayers = 3,
            IsVisible = true,  // Make the room visible in the lobby
            IsOpen = true,     // Allow other players to join the room
            CustomRoomProperties = new Hashtable { { "GameStarted", false } } // Custom room properties
        };

        PhotonNetwork.CreateRoom(roomName, roomOptions);
    }

    public void JoinRandomRoom()
    {
        PhotonNetwork.JoinRandomRoom();

    }

    public void JoinRoom(string name)
    {
        PhotonNetwork.JoinRoom(name);

    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    public override void OnConnectedToMaster() => PhotonNetwork.JoinLobby();

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        int roomCount = roomList.Count;
        for (int i = 0; i < roomCount; i++)
        {
            if (!roomList[i].RemovedFromList)
            {
                if (!_roomList.Contains(roomList[i])) _roomList.Add(roomList[i]);
                else _roomList[_roomList.IndexOf(roomList[i])] = roomList[i];
            }
            else if (_roomList.IndexOf(roomList[i]) != -1) _roomList.RemoveAt(_roomList.IndexOf(roomList[i]));
        }
    }

    public override void OnJoinedLobby()
    {
        _roomList.Clear();
        _connectCallback?.Invoke();
        _connectCallback = null;
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        //PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnJoinedRoom()
    {
        // chk is Started
        if (PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue("GameStarted", out object value))
        {
            if (value.Equals(true))
            {
                return;
            }
        }

        int actorNumber = PhotonNetwork.LocalPlayer.ActorNumber;

        Player[] sortedPlayers = PhotonNetwork.PlayerList;

        for (int i = 0; i < sortedPlayers.Length; i += 1)
        {
            if (sortedPlayers[i].ActorNumber == actorNumber)
            {
                PlayerId = i;
                break;
            }
        }

        UIManager.Instance.OpenGUI<GUINetworkRoom>("Network_Room");

    }

    public override void OnCreateRoomFailed(short returnCode, string message) {
        CreateRoom("");
    }

    public override void OnJoinRandomFailed(short returnCode, string message) {
        CreateRoom("");
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Room.NewPlayerEnter(new RoomMember(newPlayer.NickName));

    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Room.PlayerExit(new RoomMember(otherPlayer.NickName));
    }

}