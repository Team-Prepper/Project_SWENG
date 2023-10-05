using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoSingleton<UnitManager>
{

    public Unit selectedUnit;
    private Hex previouslySelectedHex;

    // Unit Selected
    public void HandleUnitSelected(GameObject unit)
    {
        Debug.Log("Name : " + unit.name);

        //UIManager.OpenGUI<GUI_ActionSelect>("ActionSelect").Set(unit);

        Unit unitReference = unit.GetComponent<Unit>();

        return;

        //if (GameManager.Instance.gamePhase == GameManager.Phase.AttackPhase)
        //{
        //    AttackManager.Instance.HideAtkRange();
        //    GameManager.Instance.gamePhase = GameManager.Phase.ActionPhase;
        //    return;
        //}
        
        if (CheckIfTheSameUnitSelected(unitReference))
            return;

        PrepareUnitForMovement(unitReference);
    }

    private bool CheckIfTheSameUnitSelected(Unit unitReference)
    {
        if (this.selectedUnit == unitReference)
        {
            ClearOldSelection();
            return true;
        }
        return false;
    }

    // Terrain Selected
    public void HandleTerrainSelected(GameObject HexGo)
    {
        Hex selectedHex = HexGo.GetComponent<Hex>();

        if(AttackManager.Instance.isAtkReady)
        {
            if (AttackManager.Instance.IsHexInAtkRange(selectedHex.HexCoords))
            {
                Character player = GameManager.Instance.player.GetComponent<PlayerManger>();
                AttackManager.Instance.BaseAtkHandler(player, selectedHex);
                AttackManager.Instance.HideAtkRange();
            }
            else
            {
                AttackManager.Instance.HideAtkRange();
            }
            return;
        }
        
        if (selectedUnit == null)
            return;
        
        if (HandleHexOutOfRange(selectedHex.HexCoords))
            return;

        if (HandleSelectedHexIsUnitHex(selectedHex.HexCoords))
            return;

        HandleTargetHexSelected(selectedHex);

    }

    private void PrepareUnitForMovement(Unit unitReference)
    {
        if (this.selectedUnit != null)
        {
            ClearOldSelection();
        }

        this.selectedUnit = unitReference;
        this.selectedUnit.Select();
        MovementSystem.Instance.ShowRange(this.selectedUnit); // cal BFS
    }

    private void ClearOldSelection()
    {
        previouslySelectedHex = null;
        this.selectedUnit.Deselect();
        MovementSystem.Instance.HideRange();
        this.selectedUnit = null;

    }

    private void HandleTargetHexSelected(Hex selectedHex)
    {
        if (previouslySelectedHex == null || previouslySelectedHex != selectedHex)
        {
            // ask about Path
            previouslySelectedHex = selectedHex;
            MovementSystem.Instance.ShowPath(selectedHex.HexCoords);
        }
        else
        {
            // Move Unit
            MovementSystem.Instance.MoveUnit(selectedUnit);
            ClearOldSelection();
        }
    }

    private bool HandleSelectedHexIsUnitHex(Vector3Int hexPosition)
    {
        if (hexPosition == HexGrid.Instance.GetClosestHex(selectedUnit.transform.position))
        {
            // hex in unit
            selectedUnit.Deselect();
            ClearOldSelection();
            return true;
        }
        return false;
    }

    private bool HandleHexOutOfRange(Vector3Int hexPosition)
    {
        if (MovementSystem.Instance.IsHexInRange(hexPosition) == false)
        {
            Debug.Log("Hex Out of range!");
            return true;
        }
        return false;
    }
}
