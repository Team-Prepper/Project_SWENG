using EHTool;
using Photon.Pun;
using System;
using System.Collections.Generic;
using UnityEngine;

public class PhotonGameSetting : MonoBehaviourPun, IPunObservable, INetworkRoom {

    public string Name => PhotonNetwork.CurrentRoom.Name;
    public int MaxPlayerCnt => PhotonNetwork.CurrentRoom.MaxPlayers;

    public List<IGameSetting.PlayerSetting> Looking;

    public IList<IGameSetting.PlayerSetting> Players { get; set; }
    public IList<string> Enemy { get; private set; }
    public IList<string> BossEnemy { get; private set; }
    public IList<string> AllCharacters { get; private set; }

    PhotonView _view;

    ISet<RoomObserver> _observers;

    public void SetPlayer(int idx, string CharacterCode)
    {
        Players[idx].PlayerCharacter = CharacterCode;
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
        _observers = new HashSet<RoomObserver>();

        IDictionaryConnector<string, string> gameData =
            new JsonDictionaryConnector<string, string>();

        IDictionary<string, string> gameDataDict =
            gameData.ReadData("GameData");

        Players = new List<IGameSetting.PlayerSetting>();

        IDictionaryConnector<string, List<string>> enemyData =
            new JsonDictionaryConnector<string, List<string>>();

        IDictionary<string, List<string>> enemyDataDict =
            enemyData.ReadData("EnemyData");

        Enemy = enemyDataDict["Enemy"];
        BossEnemy = enemyDataDict["BossEnemy"];
        AllCharacters = enemyDataDict["AllCharacters"];

    }

    public IDisposable Subscribe(RoomObserver o)
    {

        if (!_observers.Contains(o))
        {
            _observers.Add(o);

            o.Renewal();
        }

        return new RoomUnsubscriber(_observers, o);
    }

    void Renewal()
    {
        Debug.Log("Renewal");

        foreach (RoomObserver r in _observers)
        {
            r.Renewal();
        }

        Looking = new List<IGameSetting.PlayerSetting>();

        foreach (var p in Players) {
            Looking.Add(p);
        }

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

    public void NewPlayerEnter(RoomMember newMember)
    {
        if (!_view.IsMine) return;

        _view.RPC("PlayerOnRoom", RpcTarget.MasterClient, newMember.NickName);

    }

    [PunRPC]
    private void PlayerOnRoom(string name) {

        GameManager.Instance.Network.Chat.SendSystemMsg(
            string.Format("{0} Enter", name));
        Players.Add(new IGameSetting.PlayerSetting(name, "Player"));
        Renewal();

    }

    public void PlayerExit(RoomMember exitMember)
    {
        if (!_view.IsMine) return;
        _view.RPC("PlayerOffRoom", RpcTarget.MasterClient, exitMember.NickName);
    }

    [PunRPC]
    private void PlayerOffRoom(string name)
    {

        GameManager.Instance.Network.Chat.SendSystemMsg(
            string.Format("{0} Exit", name));

        Players.RemoveAt(FindIdxByName(name));
        Renewal();

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
            Renewal();
            //stream.SendNext(Enemy.Count);
            //stream.SendNext(BossEnemy.Count);
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
        Renewal();
    }
}