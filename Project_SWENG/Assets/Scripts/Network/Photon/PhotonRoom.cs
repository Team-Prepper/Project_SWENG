using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PhotonRoom : MonoBehaviour, INetworkRoom {

    PhotonView _view;

    Dictionary<string, bool> _playerReadyState = new Dictionary<string, bool>();
    ISet<RoomObserver> _observers;

    public int ReadyState { get; private set; }

    public string Name => PhotonNetwork.CurrentRoom.Name;

    public int PlayerCount => PhotonNetwork.CurrentRoom.PlayerCount;

    public int MaxPlayers => PhotonNetwork.CurrentRoom.MaxPlayers;

    int _readyPlayerCount;
    bool _isReady;

    void Awake() {
        _view = GetComponent<PhotonView>();
        _observers = new HashSet<RoomObserver>();
    }

    public IDisposable Subscribe(RoomObserver o) {

        if (!_observers.Contains(o))
        {
            _observers.Add(o);

            o.Renewal();
        }

        return new RoomUnsubscriber(_observers, o);
    }

    void RoomStateChange()
    {

        foreach (RoomObserver r in _observers)
        {
            r.Renewal();
        }

    }

    public bool IsIdxPlayerReady(int idx)
    {
        return ((1 << idx) & ReadyState) > 0;
    }

    public RoomMember[] RoomMemberList()
    {
        IList<RoomMember> members = new List<RoomMember>();

        foreach (Player p in PhotonNetwork.PlayerList) {
            members.Add(new RoomMember(p.NickName));
        }
        return members.ToArray();
    }

    [PunRPC]
    void ReadyStateChange(int state)
    {
        ReadyState = state;

        RoomStateChange();
    }

    public void Ready()
    {
        if (_isReady) return;
        _isReady = true;
        _view.RPC("ReadyToServer", RpcTarget.MasterClient, PhotonNetwork.NickName);
    }
    public void ReadyCancel()
    {
        if (!_isReady) return;
        _isReady = false;
        _view.RPC("ReadyCancleToServer", RpcTarget.MasterClient, PhotonNetwork.NickName);

    }

    [PunRPC]
    private void ReadyToServer(string name)
    {
        _readyPlayerCount++;
        _playerReadyState[name] = true;
        _view.RPC("ReadyStateChange", RpcTarget.All, _ReadyStateInt());
    }

    [PunRPC]
    private void ReadyCancleToServer(string name)
    {
        _readyPlayerCount--;
        _playerReadyState[name] = false;
        _view.RPC("ReadyStateChange", RpcTarget.All, _ReadyStateInt());
    }

    private int _ReadyStateInt()
    {

        int retval = 0;

        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            Debug.Log(PhotonNetwork.PlayerList[i].NickName);
            if (PhotonNetwork.PlayerList[i].NickName.Equals(PhotonNetwork.MasterClient.NickName)) continue;
            if (!_playerReadyState.ContainsKey(PhotonNetwork.PlayerList[i].NickName)) continue;
            if (!_playerReadyState[PhotonNetwork.PlayerList[i].NickName]) continue;

            retval += (1 << i);
        }

        Debug.Log(retval);

        return retval;

    }

    public bool StartGame()
    {
        if (!PhotonNetwork.IsMasterClient) return false;
        if (_readyPlayerCount + 1 < PhotonNetwork.PlayerList.Length) return false;

        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            if (PhotonNetwork.PlayerList[i].NickName.Equals(PhotonNetwork.MasterClient.NickName)) continue;
            if (!_playerReadyState.ContainsKey(PhotonNetwork.PlayerList[i].NickName)) return false;
            if (!_playerReadyState[PhotonNetwork.PlayerList[i].NickName]) return false;
        }

        PhotonNetwork.LoadLevel(1);

        return true;
    }

    public void NewPlayerEnter(RoomMember newMember)
    {
        RoomStateChange();

        GameManager.Instance.Network.Chat.SendSystemMsg(
            string.Format("{0} Enter", newMember.NickName));

        if (_view.IsMine)
        {
            _view.RPC("PlayerOnRoom", RpcTarget.MasterClient, PhotonNetwork.NickName);
        }

    }

    public void PlayerExit(RoomMember exitMember) {

        RoomStateChange();

        GameManager.Instance.Network.Chat.SendSystemMsg(
            string.Format("{0} Exit", exitMember.NickName));

        if (_view.IsMine)
        {
            _playerReadyState.Remove(exitMember.NickName);
            _view.RPC("ReadyStateChange", RpcTarget.All, _ReadyStateInt());
        }
    }

    [PunRPC]
    private void PlayerOnRoom(string nickName)
    {
        _playerReadyState.Add(nickName, false);
    }
}