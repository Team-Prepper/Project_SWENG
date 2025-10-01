using System.Collections.Generic;
using UnityEngine;

namespace SWEng
{
    public class BasicSkill : ISkill
    {

        private ICharacter _cc;

        private int _value;

        public BasicSkill(int value)
        {
            _value = value;

        }

        public void Set(ICharacterController selector, ICharacter cc)
        {
            _cc = cc;
            new RangeTargetSelector().Set(Attack, _cc);
        }

        public void Attack(IList<IDamageable> targets)
        {
            Vector3 look = Vector3.forward;

            foreach (var target in targets)
            {
                if (target == null) continue;

                target.Status.TakeDamage(_value);
                look = target.transform.position;

            }

            _cc.transform.LookAt(look + _cc.transform.position.y * Vector3.up);

            _cc.DicePoint.UsePoint(_value);
            _cc.CamController.CamSetting("Wide");
            _cc.Animation.PlayAnim("SetTrigger", "Attack");
            _cc.ActionEnd(2f);

        }

    }
}