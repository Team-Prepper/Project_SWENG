using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUI_DiceSetter : MonoBehaviour
{
    NetworkManager _network;
    [SerializeField] Image baseDicePoint;
    [SerializeField] List<Sprite> sprites = new List<Sprite>();
    void Start()
    {
        _network = GameObject.Find("NetworkManager").GetComponent<NetworkManager>();
    }

    public void SetBaseDicePoint(int value)
    {
        _network.SetBaseDicePointHandler(value);
        SetPointImg(value);
    }

    private void SetPointImg(int value)
    {
        int index = 0;
        switch (value)
        {
            case 20:
                index = 0; break;
            case 15:
                index = 1; break;
            case 12:
                index = 2; break;
            case 10:
                index = 3; break;
            case 5:
                index = 4; break;
            case 0:
                index = 5; break;
            default:
                index = 5; break;
        }
        baseDicePoint.sprite = sprites[index];
    }
}
