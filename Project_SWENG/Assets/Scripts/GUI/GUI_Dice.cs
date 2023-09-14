using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GUI_Dice : GUIPopUp
{
    [SerializeField] private Unit _targetPlayer;

    [SerializeField] private Dice _dice;


    protected override void Open()
    {
        base.Open();
    }

    public void SetPlayer(Unit target) {

        _targetPlayer = target;

    }

    public void SetDiceValue()
    {
        _targetPlayer.dicePoints = _dice.Value;
        Close();
    }

    public override void Close()
    {
        gameObject.SetActive(false);
    }

}
