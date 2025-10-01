using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SWEng.Network;
using EasyH.Unity.UI;

public class GUINetworkLobby : GUIFullScreen
{
    [SerializeField] private Text _lobbyInfor;        // TMP_Text->Text
    [SerializeField] private GUINetworkLobbyRoom[] _btnEnterRoom;
    [SerializeField] private Button PreviousBtn;
    [SerializeField] private Button NextBtn;

    private IList<NetworkRoom> _rooms;
    private int _currentPage = 1;

    public override void Open()
    {
        base.Open();

        UpdateData();

    }

    public void UpdateData()
    {
        _lobbyInfor.text = string.Format("{0}Lobby/ {1}Connection",
            NetworkManager.Instance.System.CountOfLobbyPlayers,
            NetworkManager.Instance.System.CountOfPlayers);

        _rooms = NetworkManager.Instance.System.GetRoomInfor();

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
        NetworkManager.Instance.System.JoinRandomRoom();
    }

    public void JoinRoom(int num)
    {
        NetworkManager.Instance.System.JoinRoom(
            _rooms[(_currentPage - 1) * _btnEnterRoom.Length + num].Name);
    }

    public void Disconnect()
    {
        NetworkManager.Instance.System.Disconnect();
        Close();
    }

}
