using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUINetworkLobby : GUICustomFullScreen
{

    [SerializeField] private Text _lobbyInfor;        // TMP_Text->Text
    [SerializeField] private GUINetworkLobbyRoom[] _btnEnterRoom;
    [SerializeField] private Button PreviousBtn;
    [SerializeField] private Button NextBtn;

    IList<NetworkRoom> _rooms;
    int _currentPage = 1;

    public override void Open()
    {
        base.Open();

        UpdateData();

    }

    public void UpdateData()
    {
        _lobbyInfor.text = string.Format("{0}Lobby/ {1}Connection",
            GameManager.Instance.Network.CountOfLobbyPlayers,
            GameManager.Instance.Network.CountOfPlayers);

        _rooms = GameManager.Instance.Network.GetRoomInfor();

        int maxPage = (_rooms.Count % _btnEnterRoom.Length == 0) ?
            _rooms.Count / _btnEnterRoom.Length : _rooms.Count / _btnEnterRoom.Length + 1;

        PreviousBtn.interactable = _currentPage > 1;
        NextBtn.interactable = _currentPage < maxPage;

        int _multiple = (_currentPage - 1) * _btnEnterRoom.Length;

        for (int i = 0; i < _btnEnterRoom.Length; i++)
        {
            if (_multiple + i >= _rooms.Count)
            {
                _btnEnterRoom[i].SetDisable();
                continue;
            }
            NetworkRoom target = _rooms[_multiple + i];

            _btnEnterRoom[i].SetInfor(target.Name, target.PlayerCount, target.MaxPlayers);

        }
    }

    public void JoinRandomRoom()
    {
        GameManager.Instance.Network.JoinRandomRoom();
    }

    public void JoinRoom(int num)
    {
        GameManager.Instance.Network.JoinRoom(
            _rooms[(_currentPage - 1) * _btnEnterRoom.Length + num].Name);
    }

    public void Disconnect()
    {
        GameManager.Instance.Network.Disconnect();
        Close();
    }

}
