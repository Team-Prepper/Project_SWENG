using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour, IDicePoint {

    public CharacterStatus Status { get; private set; }

    [SerializeField] int _dicePoint = 0;

    ICharacterController _cc;

    CharacterMove _moveComp;
    CharacterAttack _attackComp;

    public void SetCC(ICharacterController cc)
    {
        _cc = cc;
    }

    public void UsePoint(int usingAmount)
    {
        if (_dicePoint < usingAmount)
        {
            return;
        }
        _dicePoint -= usingAmount;
    }

    public int GetPoint()
    {
        return _dicePoint;
    }

    public virtual void SetPoint(int setValue)
    {
        _dicePoint = setValue;
        _cc.RollDice = true;
        _cc.ActionEnd(0);
    }

    public void Initial(ICharacterController cc)
    {
        _cc = cc;

        Status = gameObject.GetComponent<CharacterStatus>();
        _moveComp = gameObject.GetComponent<CharacterMove>();
        _attackComp = gameObject.GetComponent<CharacterAttack>();

        if (_moveComp == null)
            _moveComp = gameObject.AddComponent<CharacterMove>();
        if (_attackComp == null)
            _attackComp = gameObject.AddComponent<CharacterAttack>();

        Status.SetCC(_cc);
        _moveComp.SetCC(_cc);
        _attackComp.SetCC(_cc);
    }

    public void Move(Queue<Vector3> path) {
        _moveComp.Move(path);
    }

    public void Interaction(HexCoordinate targetPos)
    {
        HexGrid.Instance.GetMapUnitAt(targetPos).Interaction(_cc);
        _cc.Character.UsePoint(1);
    }
    public void EquipItem(string targetItem)
    {

    }

    public void PlayAnim(string triggerType, string triggerValue) {
        Status.PlayAnim(triggerType, triggerValue);
    }

    public IList<IActionSelector.Action> GetActionList()
    {
        List<IActionSelector.Action> list = new List<IActionSelector.Action>();

        if (_cc.RollDice == false) list.Add(IActionSelector.Action.Dice);
        if (GetPoint() > 0) list.Add(IActionSelector.Action.Interaction);

        _moveComp.TryAddAction(list);
        _attackComp.TryAddAction(list);

        return list;

    }


}