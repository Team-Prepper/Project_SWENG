using EasyH.UI;
using SWEng.GamePlay;
using SWEng.MultiPlay.Photon;
using Photon.Pun;

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
