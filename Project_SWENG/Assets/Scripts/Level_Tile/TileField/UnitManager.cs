using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoSingleton<UnitManager>
{

    [SerializeField]
    private MovementSystem movementSystem;

    public bool PlayersTurn { get; private set; } = true;

    public Unit selectedUnit;
    private Hex previouslySelectedHex;

    private void Awake()
    {

    }
    // Unit Selected
    public void HandleUnitSelected(GameObject unit)
    {
        if (PlayersTurn == false)
            return;

        Unit unitReference = unit.GetComponent<Unit>();

        if (AttackManager.Instance.isAtkReady)
        {
            AttackManager.Instance.HideAtkRange();
            return;
        }

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
        if (PlayersTurn == false)
        {
            return;
        }

        if(AttackManager.Instance.isAtkReady)
        {
            if (AttackManager.Instance.IsHexInAtkRange(selectedHex.HexCoords))
            {
                AttackManager.Instance.BaseAtkHandler();
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
        movementSystem.ShowRange(this.selectedUnit); // cal BFS
    }

    private void ClearOldSelection()
    {
        previouslySelectedHex = null;
        this.selectedUnit.Deselect();
        movementSystem.HideRange();
        this.selectedUnit = null;

    }

    private void HandleTargetHexSelected(Hex selectedHex)
    {
        if (previouslySelectedHex == null || previouslySelectedHex != selectedHex)
        {
            // ask about Path
            previouslySelectedHex = selectedHex;
            movementSystem.ShowPath(selectedHex.HexCoords);
        }
        else
        {
            // Move Unit
            movementSystem.MoveUnit(selectedUnit);
            //selectedUnit.MovementFinished += ResetTurn;
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
        if (movementSystem.IsHexInRange(hexPosition) == false)
        {
            Debug.Log("Hex Out of range!");
            return true;
        }
        return false;
    }

    private void ResetTurn(Unit selectedUnit)
    {
        selectedUnit.MovementFinished -= ResetTurn;
        PlayersTurn = true;
        selectedUnit.dicePoints = 0;
        GameManager.Instance.NextPhase();
    }
}
