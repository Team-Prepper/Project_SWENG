using Photon.Pun;
using EHTool.UIKit;
using UnityEngine;
using UnityEngine.UI;

public class GUI_PhotonConnect : GUIPopUp {

    [SerializeField] private InputField _nickNameInput;

    PhotonNetworkManager _network;
    // Start is called before the first frame update

    public override void Open()
    {
        base.Open();

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
