using EHTool.UIKit;

public class GUITitle : GUIFullScreen
{

    public void StartLocalPlay() {
        GameManager.Instance.SetGameMaster<LocalGameMaster>();
        UIManager.Instance.OpenGUI<GUIMatchReadyRoom>("Network_Room");
    }

    public void NetworkConnect() {
        UIManager.Instance.OpenGUI<GUINetworkConnect>("Connect");
    }

}
