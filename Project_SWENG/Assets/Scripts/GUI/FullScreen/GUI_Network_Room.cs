using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UISystem;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows;

public class GUI_Network_Room : GUIFullScreen {

    NetworkManager _network;
    [SerializeField] GUI_Network_Room_PlayerState[] _playerStates;

    [SerializeField] Text ListText;                 // TMP_Text -> Text
    [SerializeField] Text _roomInforText;           // TMP_Text -> Text
    [SerializeField] GameObject _startBtn;
    [SerializeField] GameObject _readyBtn;
    [SerializeField] GameObject _readyCancleBtn;
    [SerializeField] private GameObject _dicePointSetter;

    bool _isReady;
 
    protected override void Open(Vector2 openPos)
    {
        base.Open(openPos);

        _network = GameObject.Find("NetworkManager").GetComponent<NetworkManager>();
        _startBtn.SetActive(PhotonNetwork.IsMasterClient);
        _dicePointSetter.SetActive(PhotonNetwork.IsMasterClient);
        
        _readyBtn.SetActive(!PhotonNetwork.IsMasterClient);
        
        _isReady = false;
        RoomRenewal();
    }

    public void RoomRenewal()
    {
        
        ListText.text = "";

        Player[] roomMember = _network.RoomMemberList();

        for (int i = 0; i < roomMember.Length; i++)
            ListText.text += roomMember[i].NickName + ((i + 1 == roomMember.Length) ? "" : ", ");
        

        for (int i = 0; i < _playerStates.Length; i++) {
            if (roomMember.Length <= i)
            {
                _playerStates[i].gameObject.SetActive(false);
                continue;
            }
            _playerStates[i].gameObject.SetActive(true);
            _playerStates[i].SetInfor(roomMember[i].NickName, _network.IsIdxPlayerReady(i));
        }

        _roomInforText.text = string.Format("{0} / {1} / {2}Max",
            PhotonNetwork.CurrentRoom.Name, PhotonNetwork.CurrentRoom.PlayerCount, PhotonNetwork.CurrentRoom.MaxPlayers);
    }

    public void StartGame() {
        if (!_network.StartGame()) {
            UIManager.Instance.DisplayMessage("notice_PlayerNotReady");
        }
    }

    public void ReadyBtn() {
        _readyBtn.SetActive(_isReady);
        _readyCancleBtn.SetActive(!_isReady);
        if (_isReady) {
            _isReady = false;
            _network.ReadyCancle();
            return;
        }
        _isReady = true;
        _network.Ready();
    }

    public void LeaveRoom() {
        PhotonNetwork.LeaveRoom();
        Close();
    }
}
