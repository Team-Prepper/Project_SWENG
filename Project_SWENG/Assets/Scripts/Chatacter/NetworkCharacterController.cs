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
                if(equipCam != null)
                    equipCam.SetActive(false);
            }
        }

        public virtual string GetName() {
            return string.Empty;
        }

        public void Attack(Vector3 targetPos, bool isSkill = false)
        {
            transform.LookAt(targetPos);
            _photonView.RPC("RunAnimation", RpcTarget.All, 0);
            AttackAct(isSkill);
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
            if (!stat.IsAlive()) return;

            stat.Damaged(damage);

            if (stat.IsAlive())
            {
                _photonView.RPC("RunAnimation", RpcTarget.All, 1);
                DamageAct();
                return;
            }

            _photonView.RPC("RunAnimation", RpcTarget.All, 2);

            DieAct();
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
