using Photon.Pun;
using EHTool.UIKit;
using UnityEngine;
using UnityEngine.UI;

public class GUINetworkConnect : GUIPopUp {

    [SerializeField] private InputField _nickNameInput;

    public override void Open()
    {
        base.Open();
    }

    public void Connect()
    {
        if (_nickNameInput.text.Equals(string.Empty))
        {
            UIManager.Instance.DisplayMessage("title_NoNicknameError");
            return;
        }

        IGUI loadingUI = UIManager.Instance.OpenGUI<GUI_Loading>("Loading");

        GameManager.Instance.Network.Connect(_nickNameInput.text, () => {
            loadingUI.Close();
            Close();
            UIManager.Instance.OpenGUI<GUINetworkLobby>("Network_Lobby");
        });

    }
}
