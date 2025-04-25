using UnityEngine;
using EHTool.UIKit;
using System;

public class GUI_Dice : GUIPopUp {

    [SerializeField] private IDicePoint _targetPlayer;
    [SerializeField] private Dice _dice;

    private Action _closeMethod;

    public void ReOpen()
    {
        SetOn();
        PopUpAction();
    }

    public void SetPlayer(IDicePoint target)
    {
        _targetPlayer = target;
    }

    public void SetDiceValue()
    {
        _targetPlayer.SetPoint(_dice.Value);
        Close();
    }
    
    public void AddCloseMethod(Action closeMethod) {
        _closeMethod = closeMethod;
    }

    public override void Close()
    {
        gameObject.SetActive(false);
        _closeMethod?.Invoke();
        UIManager.Instance.NowDisplay.ClosePopUp(this);
    }

}