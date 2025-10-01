using UnityEngine;
using System;
using EasyH.Unity.UI;
using BKTools.Gaming.Dice;

public class GUIDice : GUIPopUp {

    [SerializeField] private DiceBase _dice;

    private Action<int> _rollMethod;
    private Action _closeMethod;

    public void ReOpen()
    {
        SetOn();
        PopUpAction();
    }

    public void SetRollMethod(Action<int> callback)
    {
        _rollMethod = callback;
    }
    
    public void Roll()
    {
        _dice.Roll(_rollMethod);
    }

    public void AddCloseMethod(Action closeMethod)
    {
        _closeMethod = closeMethod;
    }

    public override void Close()
    {
        gameObject.SetActive(false);
        _closeMethod?.Invoke();
        UIManager.Instance.NowDisplay.ClosePopUp(this);
    }

}