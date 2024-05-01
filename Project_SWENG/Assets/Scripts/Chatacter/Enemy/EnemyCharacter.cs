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

    enum State {
        ready, move, attack
    }

    State _state;

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
        curHex = HexGrid.Instance.GetTileAt(this.gameObject.transform.position);

        curHex.Entity = gameObject;

    }

    public override void SetPlay()
    {
        base.SetPlay();
        EnemyAttackHandler();
    }


    public override IList<Action> GetCanDoAction()
    {
        IList<Action> list = new List<Action>();
        bool containsPlayer = false;

        foreach (var neighbours in HexGrid.Instance.GetNeighboursFor(curHex.HexCoords, 2))
        {
            Hex curHex = HexGrid.Instance.GetTileAt(neighbours);
            GameObject entity = curHex.Entity;

            if (entity != null && entity.CompareTag("Player"))
            {
                containsPlayer = true;
            }
        }

        if (containsPlayer)
        {
            list.Add(Action.Attack);
        }

        return list;
    }

    public void EnemyAttackHandler()
    {
        List<HexCoordinate> list = new List<HexCoordinate>();

        foreach (var neighbours in HexGrid.Instance.GetNeighboursFor(curHex.HexCoords, 2))
        {
            Hex curHex = HexGrid.Instance.GetTileAt(neighbours);
            GameObject entity = curHex.Entity;

            if (entity != null && entity.CompareTag("Player"))
            {
                list.Add(neighbours);
            }
        }

        if (list.Count == 0)
        {
            TurnEnd();
            return;
        }

        _cc.Attack(list, 3);

        Invoke("TurnEnd", 3f);
    }

    private void TurnEnd()
    {

        _cc.TurnEnd();

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
        curHex.Entity = null;
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
        curHex.Item = dropitem;
        dropitem.itemHex = this.curHex;
    }

    private void DropExp()
    {
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
