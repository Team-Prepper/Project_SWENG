using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Character {

    public class ControllerOfCharacter : MonoBehaviour {

        [Header("Network")]
        [SerializeField] protected PhotonView _PhotonView;

        [SerializeField] protected Animator anim;

        [SerializeField] protected Stat stat;

        // Start is called before the first frame update
        void Start()
        {
            anim = GetComponent<Animator>();
            _PhotonView = GetComponent<PhotonView>();
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

        protected virtual void AttackAct()
        {

        }

        protected virtual void DamageAct()
        {

        }

        protected virtual void DieAct()
        {

        }
    }
}
