using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UISystem;
using UnityEngine;

public class GUI_Title : GUIFullScreen
{
    [SerializeField] private TMP_InputField _nickNameInput;

    NetworkManager _network;

    protected override void Open(Vector2 openPos)
    {
        base.Open(openPos);

        _network = GameObject.Find("NetworkManager").GetComponent<NetworkManager>();

    }
    public void Connect()
    {
        _network.Connect(_nickNameInput.text);
        PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.LocalPlayer.NickName = _nickNameInput.text;
    }
}
