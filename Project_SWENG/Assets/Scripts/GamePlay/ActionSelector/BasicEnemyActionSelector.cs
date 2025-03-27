using System.Collections.Generic;
using System.Collections;
using System.Linq;
using UnityEngine;
using System;

public class BasicEnemyActionSelector : MonoBehaviour, IActionSelector
{

    private ICharacterController _cc;

    private IDictionary<ICharacterController, IList<IActionSelector.Action>> _todoList
        = new Dictionary<ICharacterController, IList<IActionSelector.Action>>();

    private int _continuousCnt;
    private int _dicePoint = 4;

    public void SetDicePoint(int amount) {
        _dicePoint = amount;
    }

    private void SetCharacterController(ICharacterController cc)
    {
        _cc = cc;
        _continuousCnt = 0;
    }

    public void SelectTarget(ISkill attack)
    {
        new EnemyTargetSelector().Set(attack, _cc);

    }

    public void Ready(ICharacterController cc, IList<IActionSelector.Action> actionList)
    {
        if (actionList.Contains(IActionSelector.Action.Dice)) {
            cc.DicePoint.SetPoint(_dicePoint);
            return;
        }

        if (_cc == null)
        {
            SetCharacterController(cc);
        }
        if (_cc == cc)
        {
            DoAction(actionList);
            return;
        }
        _todoList.Add(cc, actionList);

    }

    IEnumerator WaitSeconds(float waitSeconds, Action action)
    {
        yield return new WaitForSeconds(waitSeconds);
        action?.Invoke();
    }

    private void DoAction(IList<IActionSelector.Action> actionList)
    {

        //Debug.LogFormat("{0}'s Turn", _cc.Status.CharacterCode);

        IList<IActionSelector.Action> list = new List<IActionSelector.Action>();

        foreach (IActionSelector.Action a in actionList)
        {
            if (a == IActionSelector.Action.Interaction) continue;
            if (a == IActionSelector.Action.Inventory) continue;
            if (a == IActionSelector.Action.Attack && null == GetEnemyInRange(_cc.HexPos, 1)) continue;
            if (a == IActionSelector.Action.Move)
            {
                if (null == GetEnemyInRange(_cc.HexPos, Mathf.Max(_cc.DicePoint.GetPoint() / 2, 3))) continue;
                if (null != GetEnemyInRange(_cc.HexPos, 1)) continue;
            }
            list.Add(a);
        }

        if (list.Count == 0 && _continuousCnt == 0)
        {
            TurnEnd();
            return;
        }

        _continuousCnt++;

        _cc.CamSetting("Character");

        StartCoroutine(WaitSeconds(1f, ActionSelect(list)));

    }

    private Action ActionSelect(IList<IActionSelector.Action> actionList)
    {

        if (actionList.Contains(IActionSelector.Action.Attack))
        {
            return DoAttack;
        }
        if (actionList.Contains(IActionSelector.Action.Move))
        {
            return DoMove;
        }

        return TurnEnd;

    }

    private void TurnEnd()
    {
        _cc.TurnEnd();
        _cc = null;

        if (_todoList.Count < 1) return;

        SetCharacterController(_todoList.ToList()[0].Key);
        DoAction(_todoList[_cc]);
        _todoList.Remove(_cc);

    }

    private void DoAttack()
    {
        _cc.Status.Skill.GetSkill().Set(this, _cc);
    }

    private void DoMove()
    {

        HexCoordinate? pos = GetEnemyInRange(
            _cc.HexPos, Mathf.Max(3, _cc.DicePoint.GetPoint() / 2));

        if (pos == null)
        {
            _cc.ActionEnd(0);
            return;
        }

        _cc.CamSetting("Wide");

        IPathGroup movementRange = HexGrid.Instance.GetPathGroupTo(
            _cc.HexPos, pos.Value, _cc.DicePoint.GetPoint() + 2);

        IList<HexCoordinate> pathHex = movementRange.GetPathTo(pos.Value);
        IList<Vector3> path = pathHex.Select(
            pos => HexGrid.Instance.GetMapUnitAt(pos).transform.position).ToList();

        path.RemoveAt(path.Count - 1);
        _cc.Move(new Queue<Vector3>(path));

    }

    public void CamSetting()
    {
        _cc.CamSetting("Character");
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