using UnityEngine;
using UnityEngine.UI;
using EHTool.UIKit;
using System;

public class GUINetworkRoom : GUICustomFullScreen, RoomObserver {

    [SerializeField] GUINetworkRoomPlayerState[] _playerStates;

    [SerializeField] Text ListText;                 // TMP_Text -> Text
    [SerializeField] Text _roomInforText;           // TMP_Text -> Text
    [SerializeField] GameObject _startBtn;
    [SerializeField] GameObject _readyBtn;
    [SerializeField] GameObject _readyCancleBtn;
    [SerializeField] private GameObject _dicePointSetter;

    bool _isReady;

#nullable enable
    private IDisposable? _cancellation;

    void OnEnable()
    {
        _cancellation = GameManager.Instance.Network.Room.Subscribe(this);

    }

    void OnDisable()
    {
        _cancellation?.Dispose();
    }
    void OnDestroy()
    {
        _cancellation?.Dispose();
    }

    public override void Open()
    {
        base.Open();

        GameManager.Instance.Network.GameMasterSet();
        _isReady = false;
        Renewal();
    }

    public void GameSetting() {
        if (GameManager.Instance.Network.IsMaster)
        {
            UIManager.Instance.OpenGUI<GUIGameSetting>("GameSetting");
            return;
        }
        UIManager.Instance.OpenGUI<GUIPlayerSetting>("PlayerSetting");
    }

    public void Renewal()
    {
        
        ListText.text = "";

        INetworkRoom curRoom = GameManager.Instance.Network.Room;

        RoomMember[] roomMember = curRoom.RoomMemberList();

        for (int i = 0; i < roomMember.Length; i++)
            ListText.text += roomMember[i].NickName + ((i + 1 == roomMember.Length) ? "" : ", ");

        for (int i = 0; i < _playerStates.Length; i++) {
            if (roomMember.Length <= i)
            {
                _playerStates[i].gameObject.SetActive(false);
                continue;
            }
            _playerStates[i].gameObject.SetActive(true);
            _playerStates[i].SetInfor(roomMember[i].NickName,
                curRoom.IsIdxPlayerReady(i));
        }

        _roomInforText.text = string.Format("{0} / {1} / {2}Max",
            curRoom.Name, curRoom.PlayerCount, curRoom.MaxPlayers);

        _startBtn.SetActive(GameManager.Instance.Network.IsMaster);
        _dicePointSetter.SetActive(GameManager.Instance.Network.IsMaster);

        _readyBtn.SetActive(!GameManager.Instance.Network.IsMaster);
    }

    public void StartGame() {
        if (!GameManager.Instance.Network.Room.StartGame()) {
            UIManager.Instance.DisplayMessage("notice_PlayerNotReady");
        }
    }

    public void ReadyBtn() {
        _readyBtn.SetActive(_isReady);
        _readyCancleBtn.SetActive(!_isReady);
        if (_isReady) {
            _isReady = false;
            GameManager.Instance.Network.Room.ReadyCancel();
            return;
        }
        _isReady = true;
        GameManager.Instance.Network.Room.Ready();
    }

    public void LeaveRoom() {
        GameManager.Instance.Network.LeaveRoom();
        Close();
    }
}
