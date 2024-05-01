using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

namespace CharacterSystem {

    public class Character : MonoBehaviour
    {
        public enum Action { 
            Dice, Move, Attack
        }

        protected ICharacterController _cc;

        [SerializeField] protected IHealthUI _healthUI;

        [SerializeField] protected GameObject equipCam;
        [SerializeField] protected Animator anim;

        public Stat stat;

        public void TakeDamage(int amount)
        {
            if (!stat.IsAlive()) return;

            stat.Damaged(amount);
            _healthUI.UpdateGUI(stat.HP);

        }

        public virtual string GetName() {
            return string.Empty;
        }
        
        public virtual int GetAttackValue()
        {
            return stat.GetAttackValue();
        }

        public virtual int GetTeamIdx() {
            return 1;
        }

        public virtual void DoAttact(int idx) {
            IAttack attack = new BasicAttack(_cc, transform.position, 3);
            attack.Attack();
        }

        public virtual void Initial(ICharacterController cc) {

            anim = GetComponent<Animator>();
            _cc = cc;
        }

        public virtual void AttackAct(bool isSkill)
        {
            RunAnimation(0);

        }

        public virtual void DamageAct()
        {
            RunAnimation(1);

        }

        public virtual void DieAct()
        {
            RunAnimation(2);

        }

        public virtual void SetPlay() { 
            
        }

        public virtual IList<Action> GetCanDoAction() {
            return null;
        }

        public void RunAnimation(int type)
        {
            switch (type)
            {
                case 0: // attack
                    anim.SetTrigger("Attack");
                    break;
                case 1: // hit
                    anim.SetTrigger("Hit");
                    break;
                case 2: // die
                    anim.SetTrigger("Die");
                    break;
                default:
                    break;
            }
        }

    }
}
