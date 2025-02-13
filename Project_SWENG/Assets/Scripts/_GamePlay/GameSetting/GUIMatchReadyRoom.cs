using UnityEngine;
using UnityEngine.UI;
using EHTool.UIKit;
using System;
using System.Collections.Generic;

public class GUIMatchReadyRoom : GUIFullScreen, RoomObserver {

    [SerializeField] GUIUnitRoomPlayerState[] _playerStates;

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

        _isReady = false;
    }

    public void GameSetting()
    {
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

        IGameSetting setting = GameManager.Instance.GameSetting;
        IList<IGameSetting.PlayerSetting> players = setting.Players;

        for (int i = 0; i < players.Count; i++)
            ListText.text += players[i].Name + ((i + 1 == players.Count) ? "" : ", ");

        for (int i = 0; i < _playerStates.Length; i++)
        {
            if (players.Count <= i)
            {
                _playerStates[i].gameObject.SetActive(false);
                continue;
            }
            _playerStates[i].gameObject.SetActive(true);
            _playerStates[i].SetInfor(players[i].Name, players[i].PlayerCharacter, players[i].IsReady);
        }

        _roomInforText.text = string.Format("{0} / {1} / {2}Max", setting.Name, setting.Players.Count, setting.MaxPlayerCnt);

        _startBtn.SetActive(GameManager.Instance.Network.IsMaster);
        _dicePointSetter.SetActive(GameManager.Instance.Network.IsMaster);

        _readyBtn.SetActive(!GameManager.Instance.Network.IsMaster);
    }

    public void StartGame()
    {
        if (!GameManager.Instance.Network.Room.StartGame())
        {
            UIManager.Instance.DisplayMessage("notice_PlayerNotReady");
        }
    }

    public void ReadyBtn()
    {
        _isReady = !_isReady;
        GameManager.Instance.GameSetting.SetPlayerReady(_isReady);
        _readyBtn.SetActive(!_isReady);
        _readyCancleBtn.SetActive(_isReady);
    }

    public void LeaveRoom()
    {
        GameManager.Instance.Network.LeaveRoom();
        Close();
    }
}
