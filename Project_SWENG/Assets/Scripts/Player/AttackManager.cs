using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackManager : MonoSingleton<AttackManager>
{
    [SerializeField] int atkPoint = 3;
    [SerializeField] int baseAtkPower = 10;
    [SerializeField] GameObject[] atkMarkers;

    List<Vector3Int> atkRange = new List<Vector3Int>();

    public bool isAtkReady = false;
    private Vector3Int atkHexPos;

    Unit player;

    // TODO : get status about player 

    public static event EventHandler<IntEventArgs> EventBaseAtk; // dicePoint

    public void ReadyToAttack()
    {
        MovementSystem.Instance.HideRange();

        player = GameManager.Instance.player.GetComponent<Unit>();
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
        isAtkReady = true;
    }

    public void BaseAtkHandler(Hex selectedHex)
    {
        if(!isAtkReady) return;

        player.dicePoints -= atkPoint;

        HideAtkRange();
        Animator ani = GameManager.Instance.player.GetComponent<Animator>();

        GameManager.Instance.player.gameObject.transform.LookAt(HexGrid.Instance.GetTileAt(atkHexPos).transform.position);

        ani.SetTrigger("DoAttack");
        EventBaseAtk?.Invoke(this, new IntEventArgs(player.dicePoints));
        Attack(selectedHex, baseAtkPower);
    }

    public void Attack(Hex selectedHex, int atkPower)
    {

        selectedHex.DamageToEntity(atkPower);
        isAtkReady = false;
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
        atkHexPos = hexPosition;
        return atkRange.Contains(hexPosition); 
    }
}
