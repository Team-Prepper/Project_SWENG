using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UISystem;


public partial class NetworkManager
{
    public void JoinRandomRoom() => PhotonNetwork.JoinRandomRoom();
    
    public void LeaveRoom() => PhotonNetwork.LeaveRoom();

    public override void OnJoinedRoom()
    {
        // chk is Started
        if (PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue("GameStarted", out object value))
        {
            if (value.Equals(true))
            {
                return;
            }
        }

        int actorNumber = PhotonNetwork.LocalPlayer.ActorNumber;

        Player[] sortedPlayers = PhotonNetwork.PlayerList;

        for (int i = 0; i < sortedPlayers.Length; i += 1)
        {
            if (sortedPlayers[i].ActorNumber == actorNumber)
            {
                PlayerID = i;
                break;
            }
        }

        _room = UIManager.OpenGUI<GUI_Network_Room>("Network_Room");

    }

    public override void OnCreateRoomFailed(short returnCode, string message) { CreateRoom(""); }

    public override void OnJoinRandomFailed(short returnCode, string message) { CreateRoom(""); }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        _room.RoomRenewal();
        ChatRPC("System", string.Format("{0} Enter", newPlayer.NickName));

        /*
        if (PhotonNetwork.IsMasterClient)
            photonView.RPC("ReadyStateChange", RpcTarget.All, _ReadyStateInt());
        */

        if(photonView.IsMine)
        {
            photonView.RPC("PlayerOnRoom", RpcTarget.MasterClient, PhotonNetwork.NickName);
        }
    }

    [PunRPC]
    private void PlayerOnRoom(string nickName)
    {
        _playerReadyState.Add(nickName, false);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        _room.RoomRenewal();
        ChatRPC("System", string.Format("{0} Exit", otherPlayer.NickName));

        if (photonView.IsMine)
        {
            _playerReadyState.Remove(otherPlayer.NickName);
            photonView.RPC("ReadyStateChange", RpcTarget.All, _ReadyStateInt());
        }
    }

    Dictionary<string, bool> _playerReadyState = new Dictionary<string, bool>();

    public int ReadyState { get; private set; }
    int _readyPlayerCount;
    bool _isReady;

    public Player[] RoomMemberList() {
        return PhotonNetwork.PlayerList;
    }

    // Turn End Button Trigger
    public void Ready()
    {
        _isReady = true;
        photonView.RPC("ReadyToServer", RpcTarget.MasterClient, PhotonNetwork.NickName);
    }

    public bool IsIdxPlayerReady(int idx)
    {
        return ((1 << idx) & ReadyState) > 0;
    }

    public void ReadyCancle()
    {
        _isReady = false;
        photonView.RPC("ReadyCancleToServer", RpcTarget.MasterClient, PhotonNetwork.NickName);
    }

    [PunRPC]
    void ReadyStateChange(int state)
    {
        ReadyState = state;
        _room.RoomRenewal();
    }

    [PunRPC]
    private void ReadyToServer(string name)
    {
        _readyPlayerCount++;
        _playerReadyState[name] = true;
        photonView.RPC("ReadyStateChange", RpcTarget.All, _ReadyStateInt());
    }
    [PunRPC]
    private void ReadyCancleToServer(string name)
    {
        _readyPlayerCount--;
        _playerReadyState[name] = false;
        photonView.RPC("ReadyStateChange", RpcTarget.All, _ReadyStateInt());
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
}
