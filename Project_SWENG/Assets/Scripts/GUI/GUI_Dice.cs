using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GUI_Dice : GUIPopUp
{
    [SerializeField] private DicePoint _targetPlayer;

    [SerializeField] private Dice _dice;


    protected override void Open(Vector2 openPos)
    {
        base.Open(new Vector2(-800,0));
    }

    public void SetPlayer(DicePoint target) {

        _targetPlayer = target;

    }

    public void SetDiceValue()
    {
        _targetPlayer.SetPoint(_dice.Value);
        GameManager.Instance.gamePhase = GameManager.Phase.ActionPhase;
        Close();
    }

    public override void Close()
    {
        gameObject.SetActive(false);
    }

}
