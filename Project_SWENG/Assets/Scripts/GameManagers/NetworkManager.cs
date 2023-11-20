using System.Collections.Generic;
using ExitGames.Client.Photon;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using TMPro;
using UISystem;
using UnityEditor.XR;
using System.Runtime.InteropServices;


public class NetworkManager : MonoBehaviourPunCallbacks
{
    private string gameVersion = "2";

    string _nickName;

    public TMP_Text StatusText;
    public PhotonView PV;
    
    public static int PlayerID = -1;
    [SerializeField] private int showID;

    List<RoomInfo> myList = new List<RoomInfo>();
    int currentPage = 1, maxPage, multiple;

    public GUI_Chat _chatting;
    public GUI_Network_Room _room;

    #region roomlist
    public void MyListClick(int num)
    {
        if (num == -2) --currentPage;
        else if (num == -1) ++currentPage;
        else PhotonNetwork.JoinRoom(myList[multiple + num].Name);
        //MyListRenewal();
    }

    public List<RoomInfo> GetRoomInfor() {
        return myList;
    }

    /*
    void MyListRenewal()
    {
        maxPage = (myList.Count % CellBtn.Length == 0) ? myList.Count / CellBtn.Length : myList.Count / CellBtn.Length + 1;

        PreviousBtn.interactable = (currentPage <= 1) ? false : true;
        NextBtn.interactable = (currentPage >= maxPage) ? false : true;

        multiple = (currentPage - 1) * CellBtn.Length;
        for (int i = 0; i < CellBtn.Length; i++)
        {
            CellBtn[i].interactable = (multiple + i < myList.Count) ? true : false;
            CellBtn[i].transform.GetChild(0).GetComponent<TMP_Text>().text = (multiple + i < myList.Count) ? myList[multiple + i].Name : "";
            CellBtn[i].transform.GetChild(1).GetComponent<TMP_Text>().text = (multiple + i < myList.Count) ? myList[multiple + i].PlayerCount + "/" + myList[multiple + i].MaxPlayers : "";
        }
    }
    */
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        int roomCount = roomList.Count;
        for (int i = 0; i < roomCount; i++)
        {
            if (!roomList[i].RemovedFromList)
            {
                if (!myList.Contains(roomList[i])) myList.Add(roomList[i]);
                else myList[myList.IndexOf(roomList[i])] = roomList[i];
            }
            else if (myList.IndexOf(roomList[i]) != -1) myList.RemoveAt(myList.IndexOf(roomList[i]));
        }
    }
    #endregion


    #region init
    void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.GameVersion = gameVersion;       
        Debug.Log(PhotonNetwork.SendRate);
    }


    private void Start()
    {
        DontDestroyOnLoad(this);
    }

    void Update()
    {
        StatusText.text = PhotonNetwork.NetworkClientState.ToString();
        //show
        showID = PlayerID;
    }

    public void Connect(string nickName) { PhotonNetwork.ConnectUsingSettings(); _nickName = nickName; }

    public override void OnConnectedToMaster() => PhotonNetwork.JoinLobby();

    public override void OnJoinedLobby()
    {
        UIManager.OpenGUI<GUI_Network_Lobby>("Network_Lobby");
        PhotonNetwork.NickName = _nickName;
        myList.Clear();
    }

    public void Disconnect() => PhotonNetwork.Disconnect();

    public override void OnDisconnected(DisconnectCause cause)
    {
        //PhotonNetwork.ConnectUsingSettings();
    }
    #endregion


    #region join

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

    public void JoinRandomRoom() => PhotonNetwork.JoinRandomRoom();
    
    public void LeaveRoom() => PhotonNetwork.LeaveRoom();

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
                PlayerID = i;
                break;
            }
        }

        _room = UIManager.OpenGUI<GUI_Network_Room>("Network_Room");

    }

    public override void OnCreateRoomFailed(short returnCode, string message) { CreateRoom(""); }

    public override void OnJoinRandomFailed(short returnCode, string message) { CreateRoom(""); }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        _room.RoomRenewal();
        ChatRPC("System", string.Format("<color=yellow>{0} Enter</color>", newPlayer.NickName));
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        _room.RoomRenewal();
        ChatRPC("System", string.Format("<color=yellow>{0} Exit</color>", otherPlayer.NickName));
    }

    #endregion

    List<string> _blockUser = new List<string>();

    public void Block(string name) {
        if (_blockUser.Contains(name)) return;
        _blockUser.Add(name);
    }


    #region chat
    public void Send(string msg)
    {
        PV.RPC("ChatRPC", RpcTarget.All, PhotonNetwork.NickName, msg);
    }

    [PunRPC]
    void ChatRPC(string sender, string msg)
    {
        if (!_chatting) return;
        if (_blockUser.Contains(sender)) return;

        _chatting.Chat(sender, msg);
    }
    #endregion

    public void StartGame()
    {
        if(PhotonNetwork.IsMasterClient)
            PhotonNetwork.LoadLevel("MapData02");
    }
}
