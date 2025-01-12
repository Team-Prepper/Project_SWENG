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

    public void Ready(IList<CharacterStatus.Action> actionList)
    {
        if (actionList.Contains(CharacterStatus.Action.Dice)) {
            _cc.SetPoint(4);
            return;
        }

        IList<CharacterStatus.Action> list = new List<CharacterStatus.Action>();

        foreach (CharacterStatus.Action a in actionList)
        {
            if (a == CharacterStatus.Action.Attack && null == GetPlayerInRange(1)) continue;
            if (a == CharacterStatus.Action.Move) {
                if (null == GetPlayerInRange(Mathf.Max(_cc.GetPoint() / 2, 3))) continue;
                if (null != GetPlayerInRange(1)) continue;
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

    public void DoAction(CharacterStatus.Action action)
    {

        switch (action) {
            case CharacterStatus.Action.Attack:
                new BasicAttack(_cc, _cc.GetPoint());
                _cc.UsePoint(_cc.GetPoint());
                return;
            case CharacterStatus.Action.Move:
                DoMove();
                return;
            case CharacterStatus.Action.Dice:
                _cc.SetPoint(4);
                return;
            default:
                _cc.TurnEnd();
                return;

        }

    }

    void DoMove() {

        HexCoordinate? pos = GetPlayerInRange(Mathf.Max(3, _cc.GetPoint() / 2));

        if (pos == null)
        {
            _cc.ActionEnd();
            return;
        }

        IPathGroup movementRange = HexGrid.Instance.GetPathGroupTo(_cc.HexPos, pos.Value, _cc.GetPoint() + 2);

        IList<HexCoordinate> pathHex = movementRange.GetPathTo(pos.Value);
        IList<Vector3> path = pathHex.Select(pos => HexGrid.Instance.GetTileAt(pos).transform.position).ToList();

        path.RemoveAt(path.Count - 1);
        _cc.Move(new Queue<Vector3>(path));

    }

    private HexCoordinate? GetPlayerInRange(int range)
    {

        foreach (HexCoordinate pos in HexGrid.Instance.GetNeighboursFor( _cc.HexPos, range))
        {
            GameObject entity = HexGrid.Instance.GetTileAt(pos).Entity;

            if (entity != null && entity.CompareTag("Player"))
            {
                return pos;
            }
        }

        return null;
    }

}