using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

namespace CharacterSystem {
    public class Character : MonoBehaviour, IDamagable
    {

        [SerializeField] protected IHealthUI _healthUI;

        [SerializeField] protected GameObject equipCam;
        [SerializeField] protected Animator anim;

        public Stat stat;

        public void TakeDamage(int amount)
        {
            if (!stat.IsAlive()) return;

            stat.Damaged(amount);

            if (stat.IsAlive())
            {
                RunAnimation(1);
                _healthUI.UpdateGUI(stat.HP);
                DamageAct();
                return;
            }

            RunAnimation(2);

            _healthUI.UpdateGUI(stat.HP);
            DieAct();
        }

        public virtual string GetName() {
            return string.Empty;
        }

        public void Attack(Vector3 targetPos, bool isSkill = false)
        {
            transform.LookAt(targetPos);
            RunAnimation(0);
            AttackAct(isSkill);
        }
        
        public virtual int GetAttackValue()
        {
            return stat.GetAttackValue();
        }

        protected virtual void AttackAct(bool isSkill)
        {

        }
   

        protected virtual void DamageAct()
        {

        }

        public virtual void DieAct()
        {

        }

        public virtual void SetPlay() { 
            
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

        // Start is called before the first frame update
        protected virtual void Start()
        {
            anim = GetComponent<Animator>();
        }
    }
}
