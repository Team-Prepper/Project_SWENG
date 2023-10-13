using System;
using System.Collections;
using System.Collections.Generic;

namespace Character {
    public class AttackManager : Singleton<AttackManager> {

        public void AttackTo(NetworkCharacterController attacker, NetworkCharacterController defender)
        {

            defender.DamagedHandler(attacker.GetAttackValue());
        }

        public void BaseAtkHandler(NetworkCharacterController attacker, Hex targetHex)
        {
            attacker.Attack(targetHex.transform.position);

            if (!targetHex.Entity || !targetHex.Entity.TryGetComponent(out NetworkCharacterController target)) return;

            AttackTo(attacker, target);
        }

    }

}