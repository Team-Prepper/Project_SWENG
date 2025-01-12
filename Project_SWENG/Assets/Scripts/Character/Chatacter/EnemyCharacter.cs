/*
using UnityEngine;
using CharacterSystem;

public class EnemyCharacter : Character {

    [SerializeField] LayerMask playerLayerMask;
    [SerializeField] GameObject _healthObject;

    public EnemyStat enemyStat;

    public Hex curHex;

    public override string GetName()
    {
        return enemyStat.monsterName;
    }

    private void OnEnable()
    {
        if (enemyStat == null)
            enemyStat = GetComponent<EnemyStat>();

        stat.SetHP(enemyStat.maxHp, enemyStat.maxHp, 0);

    }

    public override int GetAttackValue()
    {
        return enemyStat.atk;
    }

    private void DropItem()
    {
        if (enemyStat.dropItem.Count == 0) return;
        Item dropitem = enemyStat.dropItem[Random.Range(0, enemyStat.dropItem.Count)];
        //curHex.Item = dropitem;
        //dropitem.itemHex = this.curHex;
    }

    private void DropExp()
    {
        return;
        /*
        foreach (var neighbours in HexGrid.Instance.GetNeighboursFor(curHex.HexCoords))
        {
            Hex curHex = HexGrid.Instance.GetTileAt(neighbours);
            GameObject entity = curHex.Entity;
            if (entity != null && entity.CompareTag("Player"))
            {
                entity.GetComponent<PlayerCharacter>()?.GetExp(enemyStat.Exp);
            }
        }
    }
}
*/