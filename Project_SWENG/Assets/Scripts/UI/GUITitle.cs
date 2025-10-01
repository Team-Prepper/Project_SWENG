using EasyH.Unity.UI;
using MultiPlay.Photon.Network;
using SWEng;

public class GUITitle : GUIFullScreen
{

    public void StartLocalPlay() {
        GameManager.Instance.SetGameMaster<GameMaster>();
        UIManager.Instance.OpenGUI<GUIMatchReadyRoom>("Network_Room");
    }

    public void NetworkConnect()
    {
        PhotonNetworkSystem.OnSystem();

        UIManager.Instance.OpenGUI<GUINetworkConnect>("Connect");
        
    }

}
