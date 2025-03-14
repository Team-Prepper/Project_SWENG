using EHTool;
using Photon.Pun;
using System.Collections.Generic;
using System;
using UnityEngine;

public class PhotonGameSetting : MonoBehaviourPun, IPunObservable, IGameSetting {

    public string Name => PhotonNetwork.CurrentRoom.Name;
    public int MaxPlayerCnt => PhotonNetwork.CurrentRoom.MaxPlayers;

    public IList<IGameSetting.PlayerSetting> Players { get; set; }
    public IList<string> Enemy { get; private set; }
    public IList<string> BossEnemy { get; private set; }

    public bool IsMaster => PhotonNetwork.IsMasterClient;

    private PhotonView _view;
    private ISet<MatchObserver> _observers;
    private string _defaultCharacter;

    public IDisposable Subscribe(MatchObserver o)
    {

        if (!_observers.Contains(o))
        {
            _observers.Add(o);

            o.Renewal();
        }

        return new MatchUnsubscriber(_observers, o);

    }

    public void SetPlayer(int idx, string CharacterCode)
    {
        Players[idx].PlayerCharacter = CharacterCode;
    }
    
    void Notify()
    {
        foreach (MatchObserver r in _observers)
        {
            r.Renewal();
        }

    }

    public void AddPlayer(string name, string characterCode) {
        Players.Add(new IGameSetting.PlayerSetting(name, characterCode));
        Notify();
    }

    public void RemovePlayer(int idx) {
        Players.RemoveAt(idx);
    }

    public void AddEnemy(string characterCode)
    {
        Enemy.Add(characterCode);
    }

    public void RemoveEnemy(string characterCode)
    {
        Enemy.Remove(characterCode);
    }

    public void AddBossEnemy(string characterCode)
    {
        BossEnemy.Add(characterCode);

    }

    public void RemoveBossEnemy(string characterCode)
    {
        BossEnemy.Remove(characterCode);
    }

    public void Awake()
    {
        _view = GetComponent<PhotonView>();
        _observers = new HashSet<MatchObserver>();

        IDictionaryConnector<string, List<string>> gameData =
            new JsonDictionaryConnector<string, List<string>>();

        IDictionary<string, List<string>> gameDataDict =
            gameData.ReadData("DefaultGameSettingInfor");

        Players = new List<IGameSetting.PlayerSetting>();

        _defaultCharacter = gameDataDict["Player"][0];

        Enemy = gameDataDict["Enemy"];
        BossEnemy = gameDataDict["BossEnemy"];

        _view.FindObservables(true);
        

    }

    private int FindIdxByName(string name) {
        for (int i = 0; i < Players.Count; i++)
        {
            if (name.Equals(Players[i].Name)) return i;
        }

        return 0;
    }

    public void SetPlayerReady(bool v)
    {
        Debug.Log(PhotonNetwork.LocalPlayer.NickName);
        _view.RPC("ReadyToServer", RpcTarget.MasterClient, PhotonNetwork.LocalPlayer.NickName, v ? 1 : 0);
    }

    [PunRPC]
    private void ReadyToServer(string name, int value)
    {
        Players[FindIdxByName(name)].IsReady = value == 1;
    }

    public bool StartGame()
    {
        if (!PhotonNetwork.IsMasterClient) return false;

        for (int i = 0; i < Players.Count; i++)
        {
            if (!Players[i].IsReady && !Players[i].Name.Equals(PhotonNetwork.LocalPlayer.NickName)) return false;
        }

        PhotonNetwork.LoadLevel(1);

        return true;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {

        if (stream.IsWriting) {
            stream.SendNext(Players.Count);
            for (int i = 0; i < Players.Count; i++) {
                stream.SendNext(Players[i].Name);
                stream.SendNext(Players[i].PlayerCharacter);
                stream.SendNext(Players[i].IsReady);
            }
            Notify();
            return;
        }
        int cnt = (int)stream.ReceiveNext();
        Players = new List<IGameSetting.PlayerSetting>();

        for (int i = 0; i < cnt; i++) {
            string name = (string)stream.ReceiveNext();
            string cc = (string)stream.ReceiveNext();
            bool isReady = (bool)stream.ReceiveNext();
            Players.Add(new IGameSetting.PlayerSetting(name, cc, isReady));
        }
        Notify();
    }

    public void DisposeGame()
    {
        GameManager.Instance.Network.LeaveRoom();
        GameManager.Instance.GameSetting = new LocalGameSetting();
    }

}