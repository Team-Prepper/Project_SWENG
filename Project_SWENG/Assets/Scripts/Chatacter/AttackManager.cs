using System;
using System.Collections;
using System.Collections.Generic;

namespace Character {
    public class AttackManager : Singleton<AttackManager> {

        public void AttackTo(ControllerOfCharacter attacker, ControllerOfCharacter defender)
        {

            defender.DamagedHandler(attacker.GetAttackValue());
        }

        public void BaseAtkHandler(ControllerOfCharacter attacker, Hex targetHex)
        {
            attacker.Attack(targetHex.transform.position);

            if (!targetHex.Entity || !targetHex.Entity.TryGetComponent(out ControllerOfCharacter target)) return;

            AttackTo(attacker, target);
        }

    }

}