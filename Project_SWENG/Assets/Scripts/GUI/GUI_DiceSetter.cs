using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUI_DiceSetter : MonoBehaviour
{
    NetworkManager _network;
    void Start()
    {
        _network = GameObject.Find("NetworkManager").GetComponent<NetworkManager>();
    }

    public void SetBaseDicePoint(int value)
    {
        _network.SetBaseDicePoint(value);
    }
}
