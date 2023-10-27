using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

namespace Character {

    public class NetworkCharacterController : MonoBehaviourPun 
    {
        protected PhotonView _photonView;

        [SerializeField] protected GameObject equipCam;
        [SerializeField] protected Animator anim;

        public Stat stat;

        // Start is called before the first frame update
        protected virtual void Start()
        {
            anim = GetComponent<Animator>();
            _photonView = GetComponent<PhotonView>();
            SetPlayerCam();
        }

        private void SetPlayerCam()
        {
            if (_photonView.IsMine == false)
            {
                equipCam.SetActive(false);
            }
        }

        public void Attack(Vector3 targetPos)
        {
            transform.LookAt(targetPos);
            _photonView.RPC("RunAnimation", RpcTarget.All, 0);
            AttackAct();
        }
        
        public virtual int GetAttackValue()
        {
            return stat.GetAttackValue();
        }

        public void DamagedHandler(int damage)
        {
            _photonView.RPC("TakeDamaged", RpcTarget.All, damage);
        }

        [PunRPC]
        public void TakeDamaged(int damage)
        {
            if (stat.GetHP().Value <= 0) return;

            stat.GetHP().SubValue(damage);

            if (stat.GetHP().Value > 0)
            {
                _photonView.RPC("RunAnimation", RpcTarget.All, 1);
                DamageAct();
                return;
            }

            _photonView.RPC("RunAnimation", RpcTarget.All, 2);

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
