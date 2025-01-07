using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using EHTool.UIKit;

public class GUI_Title : GUICustomFullScreen
{
    [SerializeField] private InputField _nickNameInput;

    PhotonNetworkManager _network;

    public override void Open()
    {
        base.Open();

        _network = GameObject.Find("NetworkManager").GetComponent<PhotonNetworkManager>();

    }

    public void OpenScene(string sceneName) {
        UIManager.Instance.OpenGUI<GUI_Loading>("Loading");
        SceneManager.LoadSceneAsync(sceneName);
    }

    public void NetworkConnect() {
        UIManager.Instance.OpenGUI<GUI_PhotonConnect>("Connect");
    }

    public void DisplayMessage(string msg) {

        UIManager.Instance.DisplayMessage(msg);
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
