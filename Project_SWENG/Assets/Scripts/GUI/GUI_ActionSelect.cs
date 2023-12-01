using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UISystem;
using Character;

public class GUI_ActionSelect : GUIPopUp
{
    GameObject _target;

    DicePoint _targetUnit;
    PlayerController _playerController;

    private Item _curEquipWeapon;

    [SerializeField] Button btnAttack;
    [SerializeField] Button btnSkill;
    [SerializeField] Button btnMove;
    [SerializeField] private GameObject btnSkillObj;
    
    public void Set(GameObject target)
    {
        _target = target;
        _targetUnit = target.GetComponent<DicePoint>();
        _curEquipWeapon = target.GetComponent<EquipManager>().GetEquipWeaponHasSkill();
        _playerController = target.GetComponent<PlayerController>();
        CamMovement.Instance.IsPlayerMove = true;

        btnSkillObj.SetActive(_curEquipWeapon);
        btnAttack.interactable = (_targetUnit.GetPoint() >= 3);
        btnSkill.interactable = _playerController.canUseSkill;
        btnMove.interactable = (_targetUnit.GetPoint() >= 1);
    }
    
    public void OpenSkill()
    {
        if (_targetUnit.GetPoint() < _curEquipWeapon.skillCost) return;
        _targetUnit.UsePoint(_curEquipWeapon.skillCost);
        UIManager.OpenGUI<GUI_Attack>("Attack").Set(_target, _curEquipWeapon.skillDmg);
        //_playerController.canUseSkill = false;
        CamMovement.Instance.IsPlayerMove = true;
        _AfterAction();
    }

    public void OpenAttack()
    {
        if (_targetUnit.GetPoint() < 3) return;
        UIManager.OpenGUI<GUI_Attack>("Attack").Set(_target);
        CamMovement.Instance.IsPlayerMove = true;
        _AfterAction();
    }
    public void OpenMove()
    {
        if (_targetUnit.GetPoint() < 2) return;

        UIManager.OpenGUI<GUI_Moving>("Move").Set(_target);
        CamMovement.Instance.IsPlayerMove = false;
        _AfterAction();
    }


    private void _AfterAction() {
        Close();
    }

    /*
    public override void HexSelect(HexCoordinate selectGridPos)
    {
        Hex selected = HexGrid.Instance.GetTileAt(selectGridPos);

        if (selected && selected.Entity == _target)
            Close();
    }
    */
    public override void Close()
    {
        CamMovement.Instance.IsPlayerMove = false;
        base.Close();
    }
}
