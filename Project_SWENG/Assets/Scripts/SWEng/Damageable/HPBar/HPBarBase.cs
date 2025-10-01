using UnityEngine;
using System;
using SWEng;

public abstract class HPBarBase : MonoBehaviour, IObserver<IStatus>
{
    public void OnCompleted()
    {

    }

    public void OnError(Exception error)
    {

    }

    public void OnNext(IStatus value)
    {
        float ratio = (float)value.CurHP / value.MaxHP;
        SetRatio(ratio, value.CurHP, value.MaxHP);
    }

    protected abstract void SetRatio(float ratio, int cur, int max);

}