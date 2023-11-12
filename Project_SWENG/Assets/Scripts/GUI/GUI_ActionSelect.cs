using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UISystem;

public class GUI_ActionSelect : GUIFullScreen
{
    GameObject _target;

    DicePoint _targetUnit;

    [SerializeField] Image btnAttack;
    [SerializeField] Image btnMove;

    public void Set(GameObject target)
    {
        _target = target;
        _targetUnit = target.GetComponent<DicePoint>();
        CamMovement.Instance.IsPlayerMove = true;

        if (_targetUnit.GetPoint() < 3)
        {
            btnAttack.color = new Color(.5f, .5f, .5f);
        }
        if (_targetUnit.GetPoint() < 2)
        {
            btnMove.color = new Color(.5f, .5f, .5f);
        }
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

    public void EndTurn()
    {
        GameManager.Instance.EnemyTurn();
        Close();
    }

    private void _AfterAction() {
        Close();
    }

    public override void HexSelect(HexCoordinate selectGridPos)
    {
        Hex selected = HexGrid.Instance.GetTileAt(selectGridPos);

        if (selected && selected.Entity == _target)
            Close();
    }

    public override void Close()
    {
        CamMovement.Instance.IsPlayerMove = false;
        base.Close();
    }
}
