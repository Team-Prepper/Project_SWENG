using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Character {

    public class NetworkCharacterController : MonoBehaviourPun {

        [Header("Network")]
        [SerializeField] protected PhotonView _PhotonView;

        [SerializeField] protected Animator anim;

        [SerializeField] protected Stat stat;

        // Start is called before the first frame update
        protected virtual void Start()
        {
            anim = GetComponent<Animator>();
            _PhotonView = GetComponent<PhotonView>();
        }

        public void Attack(Vector3 targetPos)
        {
            transform.LookAt(targetPos);
            _PhotonView.RPC("RunAnimation", RpcTarget.All, 0);
            AttackAct();
        }

        public virtual int GetAttackValue()
        {
            return stat.attackPower;
        }

        public void DamagedHandler(int damage)
        {
            _PhotonView.RPC("TakeDamaged", RpcTarget.All, damage);
        }

        [PunRPC]
        public void TakeDamaged(int damage)
        {
            if (stat.curHP <= 0) return;

            stat.curHP -= damage;

            if (stat.curHP > 0)
            {
                _PhotonView.RPC("RunAnimation", RpcTarget.All, 1);
                DamageAct();
                return;
            }

            _PhotonView.RPC("RunAnimation", RpcTarget.All, 2);
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

        [PunRPC]
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
