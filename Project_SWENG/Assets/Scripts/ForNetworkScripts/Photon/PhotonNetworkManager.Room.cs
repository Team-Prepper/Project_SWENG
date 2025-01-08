using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using EHTool.UIKit;


public partial class PhotonNetworkManager
{

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

        _room = UIManager.Instance.OpenGUI<GUINetworkRoom>("Network_Room");

    }

    public override void OnCreateRoomFailed(short returnCode, string message) { CreateRoom(""); }

    public override void OnJoinRandomFailed(short returnCode, string message) { CreateRoom(""); }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        _room.Renewal();
        GameManager.Instance.Network.Chat.SendSystemMsg(
            string.Format("{0} Enter", newPlayer.NickName));

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


    Dictionary<string, bool> _playerReadyState = new Dictionary<string, bool>();

}
