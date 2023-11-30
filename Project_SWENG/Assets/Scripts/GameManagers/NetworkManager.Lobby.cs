using System.Collections.Generic;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UISystem;


public partial class NetworkManager
{

    List<RoomInfo> myList = new List<RoomInfo>();
    int currentPage = 1, maxPage, multiple;

    public GUI_Network_Room _room;

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

    public override void OnJoinedLobby()
    {
        UIManager.OpenGUI<GUI_Network_Lobby>("Network_Lobby");
        //PhotonNetwork.NickName = _nickName;
        myList.Clear();
    }

    public void Disconnect() => PhotonNetwork.Disconnect();

    public override void OnDisconnected(DisconnectCause cause)
    {
        //PhotonNetwork.ConnectUsingSettings();
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

}
