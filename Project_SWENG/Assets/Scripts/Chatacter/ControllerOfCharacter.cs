using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Character {

    public class ControllerOfCharacter : MonoBehaviour {

        [SerializeField] Animator anim;

        [SerializeField] protected Stat stat;

        // Start is called before the first frame update
        void Start()
        {
            anim = GetComponent<Animator>();
        }

        public void Attack(Vector3 targetPos)
        {
            transform.LookAt(targetPos);
            anim.SetBool("Attack", true);
            AttackAct();
        }

        public virtual int GetAttackValue()
        {
            return 10;
        }

        public void Damaged(int damage)
        {
            if (stat.curHP <= 0) return;

            stat.curHP -= damage;

            if (stat.curHP > 0)
            {
                anim.SetTrigger("Hit");
                DamageAct();
                return;
            }

            anim.SetTrigger("Die");
            stat.curHP = 0;

            DieAct();

        }

        public virtual void AttackAct()
        {

        }

        public virtual void DamageAct()
        {

        }

        public virtual void DieAct()
        {

        }
    }
}
