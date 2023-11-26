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

    [SerializeField] private InputField _roomNameInput;
    [SerializeField] private Text _nickName;          // TMP_Text->Text
    [SerializeField] private Text _lobbyInfor;        // TMP_Text->Text
    [SerializeField] private Button[] _btnEnterRoom;
    [SerializeField] private Button PreviousBtn;
    [SerializeField] private Button NextBtn;

    List<RoomInfo> _rooms;
    int _currentPage = 1;

    protected override void Open(Vector2 openPos)
    {
        base.Open(openPos);

        _nickName.text = PhotonNetwork.LocalPlayer.NickName;
        _network = GameObject.Find("NetworkManager").GetComponent<NetworkManager>();

        UpdateData();

    }

    public void UpdateData() {
        _lobbyInfor.text = string.Format("{0}Lobby/ {1}Connection", (PhotonNetwork.CountOfPlayers - PhotonNetwork.CountOfPlayersInRooms), PhotonNetwork.CountOfPlayers);

        _rooms = _network.GetRoomInfor();

        int maxPage = (_rooms.Count % _btnEnterRoom.Length == 0) ? _rooms.Count / _btnEnterRoom.Length : _rooms.Count / _btnEnterRoom.Length + 1;

        PreviousBtn.interactable = (_currentPage <= 1) ? false : true;
        NextBtn.interactable = (_currentPage >= maxPage) ? false : true;

        int _multiple = (_currentPage - 1) * _btnEnterRoom.Length;
        for (int i = 0; i < _btnEnterRoom.Length; i++)
        {
            _btnEnterRoom[i].interactable = (_multiple + i < _rooms.Count) ? true : false;
            _btnEnterRoom[i].transform.GetChild(0).GetComponent<TMP_Text>().text = (_multiple + i < _rooms.Count) ? _rooms[_multiple + i].Name : "";
            _btnEnterRoom[i].transform.GetChild(1).GetComponent<TMP_Text>().text = (_multiple + i < _rooms.Count) ? _rooms[_multiple + i].PlayerCount + "/" + _rooms[_multiple + i].MaxPlayers : "";
        }
    }

    public void CreateRoom()
    {
        string roomName = (_roomNameInput.text == "" ? "Room" + Random.Range(0, 100) : _roomNameInput.text);
        _network.CreateRoom(roomName);
    }

    public void JoinRandomRoom() {
        PhotonNetwork.JoinRandomRoom();
    }

    public void JoinRoom(int num) {
        PhotonNetwork.JoinRoom(_rooms[(_currentPage - 1) * _btnEnterRoom.Length + num].Name);
    }

    public void Disconnect() {
        PhotonNetwork.Disconnect();
        Close();
    }

}
