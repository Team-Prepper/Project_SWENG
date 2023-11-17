using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Character;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] EnemyController enemyController;


    private void Start()
    {
        enemyController = GetComponent<EnemyController>();
    }

    public void EnemyAttackHandler()
    {
        foreach(var neighbours in HexGrid.Instance.GetNeighboursDoubleFor(enemyController.curHex.HexCoords))
        {
            Hex curHex = HexGrid.Instance.GetTileAt(neighbours);
            GameObject entity = curHex.Entity;
            if (entity != null && entity.CompareTag("Player"))
            {
                AttackManager.Instance.BaseAtkHandler(enemyController, curHex);
            }
        }
    }
}
