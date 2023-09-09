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

    public void HideRange()
    {
        foreach (Vector3Int hexPosition in movementRange.GetRangePositions())
        {
            HexGrid.Instance.GetTileAt(hexPosition).DisableHighlight();
        }
        movementRange = new BFSResult();
        HideMoveNum();
    }

    public void ShowRange(Unit selectedUnit)
    {
        CalcualteRange(selectedUnit);

        Vector3Int unitPos = HexGrid.Instance.GetClosestHex(selectedUnit.CurPos);

        
        foreach (Vector3Int hexPosition in movementRange.GetRangePositions())
        {
            if (unitPos == hexPosition)
                continue;
            HexGrid.Instance.GetTileAt(hexPosition).EnableHighlight();
        }
    }

    public void CalcualteRange(Unit selectedUnit)
    {
        movementRange = GraphSearch.BFSGetRange(HexGrid.Instance, HexGrid.Instance.GetClosestHex(selectedUnit.CurPos), selectedUnit.movementPoints);
    }


    public void ShowPath(Vector3Int selectedHexPosition)
    {  
        if (movementRange.GetRangePositions().Contains(selectedHexPosition))
        {
            // hide
            foreach (Vector3Int hexPosition in currentPath)
            {
                HexGrid.Instance.GetTileAt(hexPosition).ResetHighlight();
                HideMoveNum();
            }
            currentPath = movementRange.GetPathTo(selectedHexPosition);
            // show
            moveNumParent.SetActive(true);


            int i = 0;
            foreach (Vector3Int hexPosition in currentPath)
            {
                Hex pathHex = HexGrid.Instance.GetTileAt(hexPosition);
                pathHex.HighlightPath();
                moveNumPrefabs[i += pathHex.cost].transform.position = pathHex.transform.position;
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

    public void MoveUnit(Unit selectedUnit)
    {
        Debug.Log("Moving unit " + selectedUnit.name);
        selectedUnit.MoveThroughPath(currentPath.Select(pos => HexGrid.Instance.GetTileAt(pos).transform.position).ToList());
        HideMoveNum();
    }

    public bool IsHexInRange(Vector3Int hexPosition)
    {
        return movementRange.IsHexPositionInRange(hexPosition);
    }
}
