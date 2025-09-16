using UnityEngine;
using EasyH.UI;
using UnityEngine.UI;
using SWEng.Network;

public class GUINetworkRoomCreate : GUIPopUp {

    [SerializeField] private InputField _roomNameInput;

    public void CreateRoom()
    {
        string roomName = (_roomNameInput.text == "" ? "Room" + Random.Range(0, 100) : _roomNameInput.text);
        NetworkManager.Instance.System.CreateRoom(roomName);
        Close();
    }

}
