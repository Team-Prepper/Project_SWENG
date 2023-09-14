using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GUI_PlayerInfor : GUIPopUp
{
    [SerializeField] private Unit _targetPlayer;
    [SerializeField] private TextMeshProUGUI _dicePoint;

    protected override void Open()
    {
        base.Open();
    }

    public void SetPlayer(Unit target) {

        _targetPlayer = target;

    }

    private void Update()
    {
        _dicePoint.text = _targetPlayer.dicePoints.ToString();
    }


}
