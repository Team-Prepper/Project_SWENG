using System.Collections.Generic;
using UnityEngine;

public class BasicSkill : ISkill {

    private ICharacterController _cc;

    private int _value;

    public BasicSkill(int value) {
        _value = value;

    }

    public void Set(IActionSelector selector, ICharacterController cc) {
        _cc = cc;
        new RangeTargetSelector().Set(this, _cc);
    }

    public void Attack(IList<IDamagable> targets, Vector3 look)
    {
        foreach (var target in targets)
        {
            if (target == null) continue;

            target.TakeDamage(_value);

        }

        _cc.transform.LookAt(look + _cc.transform.position.y * Vector3.up);

        _cc.DicePoint.UsePoint(_value);
        _cc.CamSetting("Wide");
        _cc.PlayAnim("SetTrigger", "Attack");
        _cc.ActionEnd(2f);
        
    }

}