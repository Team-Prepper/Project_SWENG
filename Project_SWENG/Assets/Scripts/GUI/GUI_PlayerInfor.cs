using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GUI_PlayerInfor : GUIFullScreen
{
    [SerializeField] private GameObject _targetPlayer;
    [SerializeField] private TextMeshProUGUI _dicePoint;
    [SerializeField] private GUI_PlayerHealth _playerHealth;

    Unit _targetUnit;

    protected override void Open(Vector2 openPos)
    {
        base.Open(openPos);
    }

    public void SetPlayer(GameObject target) {

        _targetPlayer = target;

        _targetUnit = target.GetComponent<Unit>();
        _playerHealth.SetPlayerHealth(target);

    }

    private void Update()
    {
        _dicePoint.text = _targetUnit.dicePoints.ToString();
    }

    public void TurnEndButton() {
        GameManager.Instance.PlayerTurnEnd();
    }

    public void AttackButton() {
        AttackManager.Instance.ReadyToAttack();
    }

}  
