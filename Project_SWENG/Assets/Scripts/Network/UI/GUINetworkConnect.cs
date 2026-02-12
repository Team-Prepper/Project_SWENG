using UnityEngine;
using UnityEngine.UI;
using SWEng.Network;
using EasyH.Unity.UI;

public class GUINetworkConnect : GUIPopUp
{

    [SerializeField] private InputField _nickNameInput;
    private IGUI _loadingUI;

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

        _loadingUI = UIManager.Instance.
            OpenGUI<GUI_Loading>("Loading");

        NetworkManager.Instance.System.OnConnectEvent += OnConnect;

        NetworkManager.Instance.System.Connect(
            _nickNameInput.text);
        
    }

    private void OnConnect()
    {
        NetworkManager.Instance.System.OnConnectEvent -= OnConnect;
        _loadingUI.Close();
        Close();
        UIManager.Instance.OpenGUI<GUINetworkLobby>("Network_Lobby");
    }

}
