using Photon.Pun;
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

    [SerializeField] TMP_Text ListText;
    [SerializeField] TMP_Text _roomInforText;
    [SerializeField] GameObject _startBtn;
    [SerializeField] GameObject _readyBtn;
    [SerializeField] GameObject _readyCancleBtn;

    bool _isReady;
 
    protected override void Open(Vector2 openPos)
    {
        base.Open(openPos);

        _network = GameObject.Find("NetworkManager").GetComponent<NetworkManager>();
        _startBtn.SetActive(PhotonNetwork.IsMasterClient);
        _readyBtn.SetActive(!PhotonNetwork.IsMasterClient);

        _isReady = false;
        RoomRenewal();
    }

    public void RoomRenewal()
    {
        
        ListText.text = "";

        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
            ListText.text += PhotonNetwork.PlayerList[i].NickName + ((i + 1 == PhotonNetwork.PlayerList.Length) ? "" : ", ");
        

        for (int i = 0; i < _playerStates.Length; i++) {
            if (PhotonNetwork.PlayerList.Length <= i)
            {
                _playerStates[i].gameObject.SetActive(false);
                continue;
            }
            _playerStates[i].gameObject.SetActive(true);
            _playerStates[i].SetInfor(PhotonNetwork.PlayerList[i].NickName, _network.IsIdxPlayerReady(i));
        }

        _roomInforText.text = string.Format("{0} / {1} / {2}Max",
            PhotonNetwork.CurrentRoom.Name, PhotonNetwork.CurrentRoom.PlayerCount, PhotonNetwork.CurrentRoom.MaxPlayers);
    }

    public void StartGame() {
        _network.StartGame();
    }

    public void ReadyBtn() {
        _readyBtn.SetActive(!_isReady);
        _readyCancleBtn.SetActive(_isReady);
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
