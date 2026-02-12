using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using System;
using SWEng.Network;
using EasyH.Unity.UI;

namespace MultiPlay.Photon.Network
{
    
    public class PhotonNetworkSystem : MonoBehaviourPunCallbacks, INetworkSystem
    {

        private PhotonRoom _room;

        private PhotonView _pv;

        private string gameVersion = "3";
        
        public INetworkRoom Room => _room;

        private List<RoomInfo> _roomList = new List<RoomInfo>();

        public bool IsMaster => PhotonNetwork.IsMasterClient;
        public bool IsConnect => PhotonNetwork.IsConnected;

        public string NickName => PhotonNetwork.NickName;
        public int CountOfPlayers => PhotonNetwork.CountOfPlayers;
        public int CountOfLobbyPlayers =>
            PhotonNetwork.CountOfPlayers
                - PhotonNetwork.CountOfPlayersInRooms;

        public int PlayerId { get; private set; }

        public Action OnConnectEvent { get; set; }

        public static void OnSystem()
        {
            if (NetworkManager.Instance.System != null) return;
            
            NetworkManager.Instance.System =
                GameManager.Instance.gameObject.
                    AddComponent<PhotonNetworkSystem>();
        }

        private void Awake()
        {
            PhotonNetwork.AutomaticallySyncScene = true;
            PhotonNetwork.GameVersion = gameVersion;

            _pv = gameObject.AddComponent<PhotonView>();
            _pv.ViewID = 999;

            _room = gameObject.AddComponent<PhotonRoom>();

            _pv.FindObservables(true);
            
        }

        public void Connect(string nickName)
        {
            PhotonNetwork.ConnectUsingSettings();
            PhotonNetwork.LocalPlayer.NickName = nickName;
        }

        public void Disconnect()
        {
            PhotonNetwork.Disconnect();
        }

        public IList<NetworkRoom> GetRoomInfor()
        {
            IList<NetworkRoom> rooms = new List<NetworkRoom>();

            foreach (RoomInfo room in _roomList)
            {
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
            UnityEngine.Debug.Log("LeaveRoom");
            _room.LeaveRoom();
        }

        public override void OnConnectedToMaster()
            => PhotonNetwork.JoinLobby();

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
            OnConnectEvent?.Invoke();
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            //PhotonNetwork.ConnectUsingSettings();
        }

        public override void OnJoinedRoom()
        {
            // chk is Started
            if (PhotonNetwork.CurrentRoom.CustomProperties.
                TryGetValue("GameStarted", out object value))
            {
                if (value.Equals(true))
                {
                    LeaveRoom();
                    return;
                }
            }

            base.OnJoinedRoom();
            _room.EnterRoom();

            Player[] sortedPlayers = PhotonNetwork.PlayerList;
            int actorNumber = PhotonNetwork.LocalPlayer.ActorNumber;

            for (int i = 0; i < sortedPlayers.Length; i += 1)
            {
                if (sortedPlayers[i].ActorNumber == actorNumber)
                {
                    PlayerId = i;
                    break;
                }
            }

            UIManager.Instance.OpenGUI<GUIFullScreen>("Network_Room");

            if (PhotonNetwork.IsMasterClient)
            {
                _room.NewPlayerEnter(new RoomMember(PhotonNetwork.LocalPlayer.NickName));
            }

        }

        public override void OnCreateRoomFailed(
            short returnCode, string message)
        {
            CreateRoom("");
        }

        public override void OnJoinRandomFailed(
            short returnCode, string message)
        {
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
}