using System;
using System.Collections;
using System.Collections.Generic;

namespace Character {
    public class AttackManager : Singleton<AttackManager> {

        public void AttackTo(CharacterController attacker, CharacterController defender)
        {

            defender.Damaged(attacker.GetAttackValue());
        }

        public void BaseAtkHandler(CharacterController attacker, Hex targetHex)
        {
            attacker.Attack(targetHex.transform.position);

            if (!targetHex.Entity || !targetHex.Entity.TryGetComponent(out CharacterController target)) return;

            AttackTo(attacker, target);
        }

    }

}