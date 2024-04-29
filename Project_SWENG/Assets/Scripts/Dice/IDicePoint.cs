using System;
using UnityEngine;

public interface IDicePoint {

    public void UsePoint(int usingAmount);
    public int GetPoint();
    public void SetPoint(int setValue);

}