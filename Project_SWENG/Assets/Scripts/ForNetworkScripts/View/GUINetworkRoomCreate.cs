using UnityEngine;
using EHTool.UIKit;
using UnityEngine.UI;

public class GUINetworkRoomCreate : GUIPopUp {

    [SerializeField] private InputField _roomNameInput;

    public void CreateRoom()
    {
        string roomName = (_roomNameInput.text == "" ? "Room" + Random.Range(0, 100) : _roomNameInput.text);
        GameManager.Instance.Network.CreateRoom(roomName);
    }

}
