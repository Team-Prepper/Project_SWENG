using System;
using UnityEngine;

public class DicePoint : MonoBehaviour
{

    [SerializeField] private int basePoint = 5;
    [SerializeField] int _point;

    private void Start()
    {
        basePoint = GameObject.Find("NetworkManager").GetComponent<NetworkManager>().baseDiceValue;
        SetPoint(0);
    }

    public void UsePoint(int usingAmount)
    {
        if (_point < usingAmount)
        {
            return;
        }
        _point -= usingAmount;
    }

    public int GetPoint()
    {
        return _point;
    }

    public void SetPoint(int setValue)
    {
        _point = setValue + basePoint;
    }


}