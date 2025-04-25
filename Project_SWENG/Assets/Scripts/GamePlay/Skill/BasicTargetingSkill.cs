using System.Collections.Generic;
using UnityEngine;

public class BasicTargetingSkill : ISkill
{

    private ICharacterController _cc;
    private int _usingPoint;
    private int _value;

    public BasicTargetingSkill(int value)
    {
        _value = value;
    }

    public void Set(IActionSelector selector, ICharacterController cc)
    {
        _cc = cc;
        _usingPoint = _cc.DicePoint.GetPoint();
        selector.SelectTarget(this);
    }

    public void Attack(IList<IDamagable> targets, Vector3 look)
    {
        foreach (var target in targets)
        {
            if (target == null) continue;
            target.TakeDamage(_value * _usingPoint);
        }

        _cc.transform.LookAt(look + _cc.transform.position.y * Vector3.up);

        _cc.DicePoint.UsePoint(_usingPoint);
        _cc.CamSetting("Battle");
        _cc.PlayAnim("SetTrigger", "Attack");
        _cc.ActionEnd(2f);

    }

}