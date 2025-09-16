using UnityEngine;
using UnityEngine.UI;
using EasyH.UI;
using System;
using System.Collections.Generic;
using SWEng.Data;

public class GUIMatchReadyRoom : GUIFullScreen, MatchObserver {

    [SerializeField] private GUIUnitRoomPlayerState[] _playerStates;

    [SerializeField] private Text _listText;
    [SerializeField] private Text _roomInforText;

    [SerializeField] private GameObject _startBtn;
    [SerializeField] private GameObject _readyBtn;
    [SerializeField] private GameObject _readyCancleBtn;
    [SerializeField] private GameObject _dicePointSetter;

    bool _isReady;

#nullable enable
    private IDisposable? _cancellation;

    void OnEnable()
    {
        _cancellation = GameManager.Instance.Setting.Subscribe(this);

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
        if (!GameManager.Instance.Setting.IsMaster)
        {
            return;
        }
        UIManager.Instance.OpenGUI<GUIGameSetting>("GameSetting");
    }

    public void Renewal()
    {
        _listText.text = "";

        IGameSetting setting = GameManager.Instance.Setting;
        IList<IGameSetting.PlayerSetting> players = setting.Players;

        for (int i = 0; i < players.Count; i++)
            _listText.text += players[i].Name + ((i + 1 == players.Count) ? "" : ", ");

        for (int i = 0; i < _playerStates.Length; i++)
        {
            if (players.Count <= i)
            {
                _playerStates[i].gameObject.SetActive(false);
                continue;
            }
            _playerStates[i].gameObject.SetActive(true);
            _playerStates[i].SetInfor(i, players[i].Name, players[i].PlayerCharacter, players[i].IsReady);
        }

        _roomInforText.text = string.Format("{0} / {1} / {2}Max", setting.Name, setting.Players.Count, setting.MaxPlayerCnt);

        if (GameManager.Instance.Setting.IsMaster) {
            _startBtn.SetActive(GameManager.Instance.Setting.IsMaster);
            _dicePointSetter.SetActive(GameManager.Instance.Setting.IsMaster);
            _readyBtn.SetActive(false);
            _readyCancleBtn.SetActive(false);
            return;
        }
        
        _readyBtn.SetActive(!_isReady);
        _readyCancleBtn.SetActive(_isReady);
    }

    public void StartGame()
    {
        if (!GameManager.Instance.Master.StartGame())
        {
            UIManager.Instance.DisplayMessage("notice_PlayerNotReady");
        }
    }

    public void ReadyBtn()
    {
        if (GameManager.Instance.Setting.IsMaster) return;
        _isReady = !_isReady;
        GameManager.Instance.Setting.SetPlayerReady(_isReady);
        _readyBtn.SetActive(!_isReady);
        _readyCancleBtn.SetActive(_isReady);
    }

    public void LeaveRoom()
    {
        GameManager.Instance.Master.DisposeGame();
        Close();
    }

}