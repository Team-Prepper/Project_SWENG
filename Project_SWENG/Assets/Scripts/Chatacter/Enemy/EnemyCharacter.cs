using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Scripting;
using CharacterSystem;

public class EnemyCharacter : Character {

    [SerializeField] LayerMask playerLayerMask;

    public EnemyStat enemyStat;

    public Hex curHex;

    bool _doAction = false;

    public override string GetName()
    {
        return enemyStat.monsterName;
    }

    private void OnEnable()
    {
        //Debug.Log("Enemy OnEnable");
        if (enemyStat == null)
            enemyStat = GetComponent<EnemyStat>();

        if (_healthUI == null)
            _healthUI = GetComponentInChildren<GUI_EnemyHealth>();

        //stat.HP = new GaugeValue<int>(enemyStat.maxHp, enemyStat.maxHp, 0);
        stat.SetHP(enemyStat.maxHp, enemyStat.maxHp, 0);
        //curHex = HexGrid.Instance.GetTileAt(this.gameObject.transform.position);

        //curHex.Entity = gameObject;

    }

    public override void SetPlay()
    {
        _doAction = false;
        base.SetPlay();
    }

    private bool PlayerInRange(uint range) {

        foreach (var neighbours in HexGrid.Instance.GetNeighboursFor(HexCoordinate.ConvertFromVector3(transform.position), range))
        {
            GameObject entity = HexGrid.Instance.GetTileAt(neighbours).Entity;

            if (entity != null && entity.CompareTag("Player"))
            {
                return true;
            }
        }

        return false;
    }

    public override IList<Action> GetCanDoAction()
    {
        if (_doAction) return new List<Action>();

        _doAction = true;

        IList<Action> list = new List<Action>();

        if (PlayerInRange(1))
        {
            list.Add(Action.Attack);
        }
        if (PlayerInRange(3))
        {
            list.Add(Action.Move);
        }

        return list;
    }

    public override int GetAttackValue()
    {
        return enemyStat.atk;
    }

    public override void DamageAct()
    {
        base.DamageAct();
        Collider[] colliders = Physics.OverlapSphere(transform.position, 10f, playerLayerMask);
        if (colliders.Length > 0)
            gameObject.transform.LookAt(colliders[0].transform);
    }

    public override void DieAct()
    {
        base.DieAct();
        curHex = HexGrid.Instance.GetTileAt(this.gameObject.transform.position);
        //curHex.Entity = null;
        /*
        if (PhotonNetwork.IsMasterClient)
        {
            GameManager.Instance.enemies.Remove(this.gameObject);
            if (enemyStat.isBoss)
                GameManager.Instance.bossEnemies.Remove(gameObject);
        }*/
        DropItem();
        DropExp();
        Destroy(this.gameObject, 1f);
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
