using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUI_Network_Room_PlayerState : MonoBehaviour
{

    [SerializeField] TMPro.TextMeshProUGUI _name;
    [SerializeField] GameObject _readyIcon;

    public void SetInfor(string name, bool isReady) {
        _name.text = name;
        _readyIcon.SetActive(isReady);
    }
}
