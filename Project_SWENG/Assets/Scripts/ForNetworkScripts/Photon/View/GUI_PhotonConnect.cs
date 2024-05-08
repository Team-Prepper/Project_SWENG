using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UISystem;
using UnityEngine;
using UnityEngine.UI;

public class GUI_PhotonConnect : GUIPopUp {

    [SerializeField] private InputField _nickNameInput;

    PhotonNetworkManager _network;
    // Start is called before the first frame update

    protected override void Open(Vector2 openPos)
    {
        base.Open(openPos);

        _network = GameObject.Find("NetworkManager").GetComponent<PhotonNetworkManager>();
    }

    public void Connect()
    {
        if (_nickNameInput.text.Equals(string.Empty))
        {
            UIManager.Instance.DisplayMessage("title_NoNicknameError");
            return;
        }
        _network.Connect(_nickNameInput.text);

        PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.LocalPlayer.NickName = _nickNameInput.text;

    }
}
