using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Character;
using UISystem;
using Photon.Pun;

public class GUI_PlayerInfor : GUIFullScreen {
    [SerializeField] private GameObject _target;
    [SerializeField] private TextMeshProUGUI _dicePoint;
    [SerializeField] private GUI_PlayerHealth _playerHealth;
    [SerializeField] private GameObject turnEndGlowLight;
    public Button turnEndButton;

    DicePoint _targetUnit;
    PlayerController _targetPlayer;

    protected override void Open(Vector2 openPos)
    {
        base.Open(openPos);
    }

    public void SetPlayer(GameObject target) {

        _target = target;

        _targetUnit = target.GetComponent<DicePoint>();
        _targetPlayer = target.GetComponent<PlayerController>();
        GameManager.Instance.turnEndButton = turnEndButton;
        _playerHealth.SetPlayerHealth(target);

    }

    protected override void Update()
    {
        base.Update();
        _dicePoint.text = _targetUnit.GetPoint().ToString();
        turnEndGlowLight.SetActive(turnEndButton.interactable);
    }

    public override void HexSelect(HexCoordinate selectGridPos)
    {
        if (GameManager.Instance.gamePhase == GameManager.Phase.EnemyPhase) {
            return;
        }

        Hex selected = HexGrid.Instance.GetTileAt(selectGridPos);

        if (!selected || !selected.Entity) return;

        if (selected.Entity == _target)
        {
            UIManager.OpenGUI<GUI_ActionSelect>("ActionSelect").Set(_target);
            return;
        }

        if (!selected.Entity.TryGetComponent<NetworkCharacterController>(out NetworkCharacterController target)) return;
        UIManager.OpenGUI<GUI_ShowCharacterInfor>("CharacterInfor").SetInfor(target.GetName(), target);
    }

    public void TurnEndButton() {
        if (_nowPopUp) return;
        GameManager.Instance.PlayerTurnEnd();
    }

    public void AttackButton() {
        if (_nowPopUp) return;
        if (!_targetPlayer.CanAttack()) return;
    }

    public void OpenInforPopUp() {
        if (_nowPopUp) return;
        UIManager.OpenGUI<GUI_ShowCharacterInfor>("PlayerInforPopUp").SetInfor(PhotonNetwork.NickName, _targetPlayer);
    }

}  
