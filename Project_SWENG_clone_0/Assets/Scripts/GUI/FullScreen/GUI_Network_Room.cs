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

    public GameObject RoomPanel;
    public TMP_Text ListText;
    public TMP_Text RoomInfoText;
    public GameObject StartObj;
    public GameObject ReadyObject;
 
    protected override void Open(Vector2 openPos)
    {
        base.Open(openPos);

        _network = GameObject.Find("NetworkManager").GetComponent<NetworkManager>();
        StartObj.SetActive(PhotonNetwork.IsMasterClient);
        RoomRenewal();
    }

    public void RoomRenewal()
    {
        ListText.text = "";
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
            ListText.text += PhotonNetwork.PlayerList[i].NickName + ((i + 1 == PhotonNetwork.PlayerList.Length) ? "" : ", ");
        RoomInfoText.text = PhotonNetwork.CurrentRoom.Name + " / " + PhotonNetwork.CurrentRoom.PlayerCount + " / " + PhotonNetwork.CurrentRoom.MaxPlayers + "MAX";
    }

    public void StartGame() {
        _network.StartGame();
    }

    public void LeaveRoom() {
        PhotonNetwork.LeaveRoom();
        Close();
    }
}
