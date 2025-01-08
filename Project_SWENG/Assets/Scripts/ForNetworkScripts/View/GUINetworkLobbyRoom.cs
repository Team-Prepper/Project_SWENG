using UnityEngine;
using UnityEngine.UI;

public class GUINetworkLobbyRoom : MonoBehaviour
{
    [SerializeField] Text _roomNameTxt;
    [SerializeField] Text _cntInforTxt;

    Button _btn;

    void Awake() {
        _btn = GetComponent<Button>();
    }

    public void SetDisable() {
        _btn.interactable = false;
        _roomNameTxt.text = "";
        _cntInforTxt.text = "";
    }

    public void SetInfor(string roomName, int nowCnt, int maxCnt)
    {
        _btn.interactable = true;
        _roomNameTxt.text = roomName;
        _cntInforTxt.text = string.Format("{0}/{1}", nowCnt, maxCnt);
    }
}
