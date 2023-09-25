using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackManager : MonoSingleton<AttackManager>
{
    [SerializeField] GameObject[] atkMarkers;
    

    List<Vector3Int> atkRange = new List<Vector3Int>();

    public bool isAtkReady = false;
    //private Vector3Int atkHexPos;

    Unit player;

    // TODO : get status about player

    public void ReadyToAttack()
    {
        MovementSystem.Instance.HideRange();

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
        GameManager.Instance.gamePhase = GameManager.Phase.AttackPhase;
    }

    public void AttackTo(Character attacker, Character defender) {

        defender.Damaged(attacker.GetAttackValue());
        isAtkReady = false;
    }

    public void BaseAtkHandler(Character attacker, Hex targetHex)
    {
        attacker.Attack(targetHex.transform.position);

        if (!targetHex.Entity || !targetHex.Entity.TryGetComponent(out Character target)) return;

        AttackTo(attacker, target);
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
        //atkHexPos = hexPosition;
        return atkRange.Contains(hexPosition); 
    }
    
}
