using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MovementSystem : MonoBehaviour
{
    public GameObject[] moveNumPrefabs;
    public GameObject moveNumParent;

    private BFSResult movementRange = new BFSResult();
    private List<Vector3Int> currentPath = new List<Vector3Int>();

    public void HideRange(HexGrid hexGrid)
    {
        foreach (Vector3Int hexPosition in movementRange.GetRangePositions())
        {
            hexGrid.GetTileAt(hexPosition).DisableHighlight();
        }
        movementRange = new BFSResult();
    }

    public void ShowRange(Unit selectedUnit, HexGrid hexGrid)
    {
        CalcualteRange(selectedUnit, hexGrid);

        Vector3Int unitPos = hexGrid.GetClosestHex(selectedUnit.CurPos);

        
        foreach (Vector3Int hexPosition in movementRange.GetRangePositions())
        {
            if (unitPos == hexPosition)
                continue;
            hexGrid.GetTileAt(hexPosition).EnableHighlight();
        }
    }

    public void CalcualteRange(Unit selectedUnit, HexGrid hexGrid)
    {
        movementRange = GraphSearch.BFSGetRange(hexGrid, hexGrid.GetClosestHex(selectedUnit.CurPos), selectedUnit.movementPoints);
    }


    public void ShowPath(Vector3Int selectedHexPosition, HexGrid hexGrid)
    {
        
        if (movementRange.GetRangePositions().Contains(selectedHexPosition))
        {
            // hide
            foreach (Vector3Int hexPosition in currentPath)
            {
                hexGrid.GetTileAt(hexPosition).ResetHighlight();
                HideMoveNum();
            }
            currentPath = movementRange.GetPathTo(selectedHexPosition);
            // show
            moveNumParent.SetActive(true);
            int i = 0;
            foreach (Vector3Int hexPosition in currentPath)
            {
                hexGrid.GetTileAt(hexPosition).HighlightPath();
                moveNumPrefabs[++i].transform.position = hexGrid.GetTileAt(hexPosition).transform.position;
            }
        }
    }

    private void HideMoveNum()
    {
        moveNumParent.SetActive(false);
        for (int i = 0; i < moveNumPrefabs.Length; i++)
        {
            moveNumPrefabs[i].transform.position = Vector3.zero;
        }
    }

    public void MoveUnit(Unit selectedUnit, HexGrid hexGrid)
    {
        Debug.Log("Moving unit " + selectedUnit.name);
        selectedUnit.MoveThroughPath(currentPath.Select(pos => hexGrid.GetTileAt(pos).transform.position).ToList());
        HideMoveNum();
    }

    public bool IsHexInRange(Vector3Int hexPosition)
    {
        return movementRange.IsHexPositionInRange(hexPosition);
    }
}
