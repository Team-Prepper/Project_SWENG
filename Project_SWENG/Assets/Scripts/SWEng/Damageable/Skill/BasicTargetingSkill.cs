using System.Collections.Generic;
using UnityEngine;


namespace SWEng
{
    public class BasicTargetingSkill : ISkill
    {

        private ICharacter _cc;
        private int _usingPoint;
        private int _value;

        public BasicTargetingSkill(int value)
        {
            _value = value;
        }

        public void Set(ICharacterController selector, ICharacter cc)
        {
            _cc = cc;
            _usingPoint = _cc.DicePoint.GetPoint();
            selector.SelectAttackPoint(Attack);
        }

        public void Attack(IList<IDamageable> targets)
        {
            Vector3 look = Vector3.forward;
            foreach (var target in targets)
            {
                if (target == null) continue;
                target.Status.TakeDamage(_value * _usingPoint);
                look = target.transform.position;
            }

            _cc.transform.LookAt(look + _cc.transform.position.y * Vector3.up);

            _cc.DicePoint.UsePoint(_usingPoint);
            _cc.CameraController.CamSetting("Battle");
            _cc.Animation.PlayAnim("SetTrigger", "Attack");
            _cc.ActionEnd(2f);

        }

    }
}