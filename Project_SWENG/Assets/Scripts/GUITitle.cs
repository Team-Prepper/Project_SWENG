using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using EHTool.UIKit;

public class GUITitle : GUICustomFullScreen
{
    [SerializeField] private InputField _nickNameInput;

    public void OpenScene(string sceneName) {
        UIManager.Instance.OpenGUI<GUI_Loading>("Loading");
        SceneManager.LoadSceneAsync(sceneName);
    }

    public void NetworkConnect() {
        UIManager.Instance.OpenGUI<GUINetworkConnect>("Connect");
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

        GameManager.Instance.Network.Connect(_nickNameInput.text);

    }

}
