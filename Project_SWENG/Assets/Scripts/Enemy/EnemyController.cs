using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : Character {

    [SerializeField] GUI_EnemyHealth healthGUI;

    public EnemyStat enemyStat;

    public Hex curHex;

    private void Awake()
    {
        if(enemyStat == null)
            enemyStat = GetComponent<EnemyStat>();
        
        if(healthGUI == null)
            healthGUI = GetComponentInChildren<GUI_EnemyHealth>();

        stat.curHP = enemyStat.maxHp;
    }

    public override int GetAttackValue() {
        Debug.Log(enemyStat.atk);
        return enemyStat.atk;
    }

    public override void DamageAct()
    {
        healthGUI.UpdateGUI((float)stat.curHP / enemyStat.maxHp);
    }

    public override void DieAct()
    {
        curHex = HexGrid.Instance.GetHexFromPosition(this.gameObject.transform.position);
        curHex.Entity = null;
        EnemySpawner.Instance.enemyList.Remove(this.gameObject);

        healthGUI.UpdateGUI(0);
        DropItem();
        Destroy(this.gameObject, 1f);
    }

    private void DropItem()
    {
        if (enemyStat.dropItem.Count == 0) return;
        Item dropitem = enemyStat.dropItem[Random.Range(0, enemyStat.dropItem.Count)];
        curHex.Item = dropitem;
        dropitem.itemHex = this.curHex;
    }
}
