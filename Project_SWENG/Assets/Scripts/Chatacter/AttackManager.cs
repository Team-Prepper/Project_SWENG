using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character {
    public class AttackManager : Singleton<AttackManager> {

        private void AttackTo(NetworkCharacterController attacker, NetworkCharacterController defender, int skillDmg = 0)
        {
            if (attacker.CompareTag("Player"))
            {
                int totalDmg = (skillDmg == 0) ? attacker.GetAttackValue() : skillDmg;
                GameManager.Instance.CalTotalAttackDamageHandler(totalDmg);
            }
            if(skillDmg > 0) // use skill
                defender.DamagedHandler(skillDmg);
            else             // base attack
                defender.DamagedHandler(attacker.GetAttackValue());
        }

        public void BaseAtkHandler(NetworkCharacterController attacker, Hex targetHex)
        {
            attacker.Attack(targetHex.transform.position);

            if (targetHex.Entity == null || !targetHex.Entity.TryGetComponent(out NetworkCharacterController target)) return;

            AttackTo(attacker, target);
        }

        public void SkillAtkHandler(NetworkCharacterController attacker, Hex targetHex, int skillDmg)
        {
            attacker.gameObject.GetComponent<PlayerController>().canUseSkill = false;
            attacker.Attack(targetHex.transform.position, true);

            if (targetHex.Entity == null || !targetHex.Entity.TryGetComponent(out NetworkCharacterController target)) return;

            AttackTo(attacker, target, skillDmg);
        }
    }

}