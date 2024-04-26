using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CharacterSystem {
    public class AttackManager : Singleton<AttackManager> {

        private void AttackTo(Character attacker, Character defender, int skillDmg = 0)
        {
            if (attacker.CompareTag("Player"))
            {
                int totalDmg = (skillDmg == 0) ? attacker.GetAttackValue() : skillDmg;
                //GameManager.Instance.GameMaster.CalTotalAttackDamageHandler(totalDmg);
            }
            if(skillDmg > 0) // use skill
                defender.TakeDamage(skillDmg);
            else             // base attack
                defender.TakeDamage(attacker.GetAttackValue());
        }

        public void BaseAtkHandler(Character attacker, Hex targetHex)
        {
            attacker.Attack(targetHex.transform.position);

            if (targetHex.Entity == null || !targetHex.Entity.TryGetComponent(out Character target)) return;

            AttackTo(attacker, target);
        }

        public void SkillAtkHandler(Character attacker, Hex targetHex, int skillDmg)
        {
            attacker.gameObject.GetComponent<PlayerCharacter>().canUseSkill = false;
            attacker.Attack(targetHex.transform.position, true);

            if (targetHex.Entity == null || !targetHex.Entity.TryGetComponent(out Character target)) return;

            AttackTo(attacker, target, skillDmg);
        }
    }

}