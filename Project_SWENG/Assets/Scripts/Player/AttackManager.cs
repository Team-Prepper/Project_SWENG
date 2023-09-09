using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackManager : MonoSingleton<AttackManager>
{
    [SerializeField] int atkPoint = 3;
    [SerializeField] GameObject[] atkMarkers;

    List<Vector3Int> atkRange = new List<Vector3Int>();

    public bool isAtkReady = false;
    public void AttackHandler()
    {
        MovementSystem.Instance.HideRange();


        Unit player = GameManager.Instance.player.GetComponent<Unit>();
        if (player == null || player.dicePoints < atkPoint) 
            return;

        Vector3Int curHexPos =  HexGrid.Instance.GetClosestHex(GameManager.Instance.player.transform.position);

        atkRange.Clear();
        int i = 0;
        foreach (var neighbour in HexGrid.Instance.GetNeighboursFor(curHexPos))
        {
            Hex atkHex = HexGrid.Instance.GetTileAt(neighbour);
            if (atkHex.tileType != Hex.Type.Field) continue;

            atkRange.Add(neighbour);
            atkMarkers[i++].transform.position = atkHex.transform.position;
        }
        UnitManager.Instance.selectedUnit = player;
        isAtkReady = true;
    }

    public void HideAtkRange()
    {
        for(int i = 0; i < atkMarkers.Length; i++)
        {
            atkMarkers[i].transform.localPosition = Vector3.zero;
        }
        isAtkReady = false;
        UnitManager.Instance.selectedUnit = null;
    }

    public bool IsHexInAtkRange(Vector3Int hexPosition)
    {
        return atkRange.Contains(hexPosition); 
    }
}
