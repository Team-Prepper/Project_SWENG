using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Scripting;
using CharacterSystem;
using System.Linq;

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

    private HexCoordinate? GetPlayerInRange(uint range) {

        foreach (HexCoordinate pos in HexGrid.Instance.GetNeighboursFor(HexCoordinate.ConvertFromVector3(transform.position), range))
        {
            GameObject entity = HexGrid.Instance.GetTileAt(pos).Entity;

            if (entity != null && entity.CompareTag("Player"))
            {
                return pos;
            }
        }

        return null;
    }

    public override IList<Action> GetCanDoAction()
    {
        if (_doAction) return new List<Action>();

        _doAction = true;

        IList<Action> list = new List<Action>();

        if (GetPlayerInRange(1) != null)
        {
            list.Add(Action.Attack);
        }
        if (GetPlayerInRange(3) != null)
        {
            list.Add(Action.Move);
        }

        return list;
    }

    public override void DoMove()
    {
        HexCoordinate? pos = GetPlayerInRange(3);

        if (pos == null) return;

        IPathGroup movementRange = HexGrid.Instance.GetPathGroupTo(HexCoordinate.ConvertFromVector3(transform.position), pos.Value, GetPoint());

        IList<HexCoordinate> pathHex = movementRange.GetPathTo(pos.Value);
        IList<Vector3> path = pathHex.Select(pos => HexGrid.Instance.GetTileAt(pos).transform.position).ToList();

        path.RemoveAt(path.Count - 1);

        Debug.Log(path.Count);

        Move(new Queue<Vector3>(path));
    }

    public override int GetAttackValue()
    {
        return enemyStat.atk;
    }

    public override void AttackAct(float time)
    {
        base.AttackAct(time);
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

        DropItem();
        DropExp();
        Destroy(gameObject, 1f);
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
