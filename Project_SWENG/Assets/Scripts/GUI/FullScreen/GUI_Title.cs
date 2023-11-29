using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UISystem;
using UnityEngine;
using UnityEngine.UI;

public class GUI_Title : GUIFullScreen
{
    [SerializeField] private InputField _nickNameInput;

    NetworkManager _network;

    protected override void Open(Vector2 openPos)
    {
        base.Open(openPos);

        _network = GameObject.Find("NetworkManager").GetComponent<NetworkManager>();

    }
    public void Connect()
    {
        if (_nickNameInput.text.Equals(string.Empty)) {
            UIManager.Instance.DisplayMessage("title_NoNicknameError");
            return;
        }
        _network.Connect(_nickNameInput.text);
        PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.LocalPlayer.NickName = _nickNameInput.text;
    }
}
