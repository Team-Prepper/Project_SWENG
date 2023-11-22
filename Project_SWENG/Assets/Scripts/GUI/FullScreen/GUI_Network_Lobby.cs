using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UISystem;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using Unity.VisualScripting;

public class GUI_Network_Lobby : GUIFullScreen
{

    private NetworkManager _network;

    [SerializeField] private InputField RoomInput;
    [SerializeField] private TMP_Text WelcomeText;
    [SerializeField] private TMP_Text LobbyInfoText;
    [SerializeField] private Button[] CellBtn;
    [SerializeField] private Button PreviousBtn;
    [SerializeField] private Button NextBtn;

    List<RoomInfo> _rooms;
    int _currentPage = 0;

    protected override void Open(Vector2 openPos)
    {
        base.Open(openPos);

        WelcomeText.text = PhotonNetwork.LocalPlayer.NickName;
        _network = GameObject.Find("NetworkManager").GetComponent<NetworkManager>();

    }

    public void UpdateData() {
        LobbyInfoText.text = (PhotonNetwork.CountOfPlayers - PhotonNetwork.CountOfPlayersInRooms) + "Lobby / " + PhotonNetwork.CountOfPlayers + "Connection";

        _rooms = _network.GetRoomInfor();

        int maxPage = (_rooms.Count % CellBtn.Length == 0) ? _rooms.Count / CellBtn.Length : _rooms.Count / CellBtn.Length + 1;

        PreviousBtn.interactable = (_currentPage <= 1) ? false : true;
        NextBtn.interactable = (_currentPage >= maxPage) ? false : true;

        int _multiple = (_currentPage - 1) * CellBtn.Length;
        for (int i = 0; i < CellBtn.Length; i++)
        {
            CellBtn[i].interactable = (_multiple + i < _rooms.Count) ? true : false;
            CellBtn[i].transform.GetChild(0).GetComponent<TMP_Text>().text = (_multiple + i < _rooms.Count) ? _rooms[_multiple + i].Name : "";
            CellBtn[i].transform.GetChild(1).GetComponent<TMP_Text>().text = (_multiple + i < _rooms.Count) ? _rooms[_multiple + i].PlayerCount + "/" + _rooms[_multiple + i].MaxPlayers : "";
        }
    }

    public void CreateRoom()
    {
        string roomName = (RoomInput.text == "" ? "Room" + Random.Range(0, 100) : RoomInput.text);
        _network.CreateRoom(roomName);
    }

    public void JoinRandomRoom() {
        PhotonNetwork.JoinRandomRoom();
    }

    public void JoinRoom(int num) {
        PhotonNetwork.JoinRoom(_rooms[(_currentPage - 1) * CellBtn.Length + num].Name);
    }

    public void Disconnect() {
        PhotonNetwork.Disconnect();
        Close();
    }

}
