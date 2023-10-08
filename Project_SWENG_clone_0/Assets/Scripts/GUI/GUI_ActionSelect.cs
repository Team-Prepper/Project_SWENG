using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUI_ActionSelect : GUIFullScreen
{
    GameObject _target;

    NetworkUnit _targetUnit;

    [SerializeField] Image btnAttack;
    [SerializeField] Image btnMove;

    public void Set(GameObject target)
    {
        _target = target;
        _targetUnit = target.GetComponent<NetworkUnit>();
        CamMovement.Instance.CamSetToPlayer(target);

        if (_targetUnit.dicePoints < 3)
        {
            btnAttack.color = new Color(.5f, .5f, .5f);
        }
        if (_targetUnit.dicePoints < 2)
        {
            btnMove.color = new Color(.5f, .5f, .5f);
        }
    }

    public void OpenAttack()
    {
        if (_targetUnit.dicePoints < 3) return;

        UIManager.OpenGUI<GUI_Attack>("Attack").Set(_target);
        _AfterAction();
    }
    public void OpenMove()
    {
        if (_targetUnit.dicePoints < 2) return;

        UIManager.OpenGUI<GUI_Moving>("Move").Set(_target);
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

    public override void HexSelect(Vector3Int selectGridPos)
    {
        Hex selected = HexGrid.Instance.GetTileAt(selectGridPos);

        if (selected && selected.Entity == _target)
            Close();
    }

    public override void Close()
    {
        CamMovement.Instance.ConvertMovementCamera();
        base.Close();
    }
}
