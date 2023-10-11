using System;
using UnityEngine;

public class DicePoint : MonoBehaviour {

    int _point;

    public void UsePoint(int usingAmount) {
        if (_point < usingAmount) {
            return;
        }
        _point -= usingAmount;
    }

    public int GetPoint()
    {
        return _point;
    }

    public void SetPoint(int setValue) {
        _point = setValue;
    }

                                                                  
}