using System.Collections.Generic;
using System.Collections;
using System.Linq;
using UnityEngine;
using System;
using SWEng;
using BKTools.Gaming.GridMap2D;

public class BasicEnemyCharacterController : MonoBehaviour, ICharacterController
{

    private ICharacter _cc;

    private IDictionary<ICharacter, IList<ICharacterController.Action>> _todoList
        = new Dictionary<ICharacter, IList<ICharacterController.Action>>();

    private int _continuousCnt;
    private int _dicePointMin = 4;
    private int _dicePointMax = 5;

    public void SetDicePointRange(int min, int max)
    {
        _dicePointMin = min;
        _dicePointMax = max;
    }

    private void SetCharacterController(ICharacter cc)
    {
        _cc = cc;
        _continuousCnt = 0;
    }

    public void SelectAttackPoint(Action<IList<IDamageable>> action)
    {
        new EnemyTargetSelector().Set(action, _cc);

    }

    public void Ready(ICharacter cc, IList<ICharacterController.Action> actionList)
    {
        if (actionList.Contains(ICharacterController.Action.Dice))
        {
            cc.DicePoint.SetPoint(UnityEngine.Random.Range(_dicePointMin, _dicePointMax));
            cc.IsRollDice = true;
            cc.ActionEnd(0);
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

    private void DoAction(IList<ICharacterController.Action> actionList)
    {

        //Debug.LogFormat("{0}'s Turn", _cc.Status.CharacterCode);

        IList<ICharacterController.Action> list = new List<ICharacterController.Action>();

        foreach (ICharacterController.Action a in actionList)
        {
            if (a == ICharacterController.Action.Interaction) continue;
            if (a == ICharacterController.Action.Attack &&
                null == GetEnemyInRange(_cc.EntityTransform.Pos, 1)) continue;
            if (a == ICharacterController.Action.Move)
            {
                if (null == GetEnemyInRange(_cc.EntityTransform.Pos, Mathf.Max(_cc.DicePoint.GetPoint() / 2, 3))) continue;
                if (null != GetEnemyInRange(_cc.EntityTransform.Pos, 1)) continue;
            }
            list.Add(a);
        }

        if (list.Count == 0 && _continuousCnt == 0)
        {
            TurnEnd();
            return;
        }

        _continuousCnt++;

        _cc.CamController.CamSetting("Character");

        StartCoroutine(WaitSeconds(1f, ActionSelect(list)));

    }

    private Action ActionSelect(IList<ICharacterController.Action> actionList)
    {

        if (actionList.Contains(ICharacterController.Action.Attack))
        {
            return DoAttack;
        }
        if (actionList.Contains(ICharacterController.Action.Move))
        {
            return DoMove;
        }

        return TurnEnd;

    }

    private void TurnEnd()
    {
        _cc.TurnMemberState.EndTurn();
        _cc = null;

        if (_todoList.Count < 1) return;

        SetCharacterController(_todoList.ToList()[0].Key);
        DoAction(_todoList[_cc]);
        _todoList.Remove(_cc);

    }

    private void DoAttack()
    {
        SkillManager.Instance.GetSkill(
            _cc.Stat.Skill).Set(this, _cc);
    }

    private void DoMove()
    {

        GridCoord2D? pos = GetEnemyInRange(
            _cc.EntityTransform.Pos, Mathf.Max(3, _cc.DicePoint.GetPoint() / 2));

        if (pos == null)
        {
            _cc.ActionEnd(0);
            return;
        }

        _cc.CamController.CamSetting("Wide");

        IList<GridCoord2D> path
            = EntityManager.Instance.GetPathGroupTo(
                _cc.EntityTransform.Pos,
                pos.Value, _cc.DicePoint.GetPoint() + 2);

        path.RemoveAt(0);
        path.RemoveAt(path.Count - 1);

        _cc.Move(path);

    }

    private GridCoord2D? GetEnemyInRange(
        GridCoord2D pos, int range)
    {

        foreach (GridCoord2D p in MapUnitManager.
            Instance.GetNeighboursFor(pos, range))
        {
            ICharacter entity = CharacterManager.
                Instance.GetCharacterAt(p);

            if (entity == null) continue;
            if (entity.TurnMemberState.TeamIdx
                == _cc.TurnMemberState.TeamIdx) continue;
            return p;
        }

        return null;
    }

}