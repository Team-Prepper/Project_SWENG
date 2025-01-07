using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EHTool.UIKit;
using UnityEngine.UI;

public class GUI_PhotonRoomCreate : GUIPopUp {

    private PhotonNetworkManager _network;

    [SerializeField] private InputField _roomNameInput;

    public void CreateRoom()
    {
        string roomName = (_roomNameInput.text == "" ? "Room" + Random.Range(0, 100) : _roomNameInput.text);
        _network.CreateRoom(roomName);
    }

    public void SetNetwork(PhotonNetworkManager network) {
        _network = network;
    }

}
