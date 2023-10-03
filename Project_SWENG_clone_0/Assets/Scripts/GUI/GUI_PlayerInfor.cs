using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GUI_PlayerInfor : GUIFullScreen
{
    [SerializeField] private GameObject _target;
    [SerializeField] private TextMeshProUGUI _dicePoint;
    [SerializeField] private GUI_PlayerHealth _playerHealth;

    Unit _targetUnit;
    PlayerManger _targetPlayer;

    protected override void Open(Vector2 openPos)
    {
        base.Open(openPos);
    }

    public void SetPlayer(GameObject target) {

        _target = target;

        _targetUnit = target.GetComponent<Unit>();
        _targetPlayer = target.GetComponent<PlayerManger>();
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
        if (!_targetPlayer.CanAttack()) return;
        
        AttackManager.Instance.ReadyToAttack();
    }

}  
