using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BasicEnemyActionSelector : IActionSelector {
    
    ICharacterController _cc;
    EnemyPlayer _player;

    public BasicEnemyActionSelector(EnemyPlayer player) {
        _player = player;
        _player.AddSelector(this);
    }

    public void SetCharacterController(ICharacterController cc)
    {
        _cc = cc;
    }

    public void Ready(IList<IActionSelector.Action> actionList)
    {
        if (actionList.Contains(IActionSelector.Action.Dice)) {
            _cc.SetPoint(4);
            return;
        }

        IList<IActionSelector.Action> list = new List<IActionSelector.Action>();

        foreach (IActionSelector.Action a in actionList)
        {
            if (a == IActionSelector.Action.Interaction) continue;
            if (a == IActionSelector.Action.Attack && null == GetEnemyInRange(_cc.HexPos, 1)) continue;
            if (a == IActionSelector.Action.Move) {
                if (null == GetEnemyInRange(_cc.HexPos, Mathf.Max(_cc.GetPoint() / 2, 3))) continue;
                if (null != GetEnemyInRange(_cc.HexPos, 1)) continue;
            }
            list.Add(a);
        }
        
        _player.ActionAdd(this, list);

    }

    public void Die()
    {
        _player.RemoveSelector(this);
    }

    public void CamSetting() {
        _cc.CamSetting("Character");
    }

    public void DoAction(IActionSelector.Action action)
    {

        switch (action) {
            case IActionSelector.Action.Attack:
                new BasicAttack(_cc, _cc.GetPoint());
                _cc.UsePoint(_cc.GetPoint());
                return;
            case IActionSelector.Action.Move:
                DoMove();
                return;
            case IActionSelector.Action.Dice:
                _cc.SetPoint(4);
                return;
            default:
                _cc.TurnEnd();
                return;

        }

    }

    void DoMove() {

        HexCoordinate? pos = GetEnemyInRange(_cc.HexPos, Mathf.Max(3, _cc.GetPoint() / 2));

        if (pos == null)
        {
            _cc.ActionEnd(0);
            return;
        }

        IPathGroup movementRange = HexGrid.Instance.GetPathGroupTo(_cc.HexPos, pos.Value, _cc.GetPoint() + 2);

        IList<HexCoordinate> pathHex = movementRange.GetPathTo(pos.Value);
        IList<Vector3> path = pathHex.Select(pos => HexGrid.Instance.GetMapUnitAt(pos).transform.position).ToList();

        path.RemoveAt(path.Count - 1);
        _cc.Move(new Queue<Vector3>(path));

    }

    private HexCoordinate? GetEnemyInRange(HexCoordinate pos, int range)
    {

        foreach (HexCoordinate p in HexGrid.Instance.GetNeighboursFor(pos, range))
        {
            ICharacterController entity = HexGrid.Instance.GetMapUnitAt(p).CC;

            if (entity == null) continue;
            if (entity.TeamIdx == _cc.TeamIdx) continue;

            return p;
        }

        return null;
    }

}