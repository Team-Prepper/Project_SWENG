using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using EasyH.Unity.UI;
using SWEng.Network;

public class GUIGameResult : GUIPopUp
{
    [SerializeField] private int _titleSceneIdx;
    
    public void GotoLobby() {
        SceneManager.LoadSceneAsync(_titleSceneIdx);
        GameManager.Instance.AddSceneLoadEvent(OpenUI);
    }

    public void GotoRoom() {
        SceneManager.LoadSceneAsync(_titleSceneIdx);
        GameManager.Instance.AddSceneLoadEvent(OpenUI2);
        
    }

    public static void OpenUI() {

        if (NetworkManager.Instance.System.IsConnect) {
            UIManager.Instance.OpenGUI<GUIWindow>("NetworkLobby");
        }
        GameManager.Instance.RemoveSceneLoadEvent(OpenUI);

    }
    
    public static void OpenUI2() {

        if (NetworkManager.Instance.System.IsConnect) {
            UIManager.Instance.OpenGUI<GUINetworkLobby>("Network_Lobby");
        }
        UIManager.Instance.OpenGUI<GUIWindow>("Network_Room");
        GameManager.Instance.RemoveSceneLoadEvent(OpenUI2);

    }

}
