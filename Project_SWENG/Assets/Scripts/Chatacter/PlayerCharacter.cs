using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character {

    public class PlayerCharacter : NetworkCharacterController,
        ICharacter, IDamagable, IDicePoint  {

        [SerializeField] int _usePointAtAttack = 3;
        [SerializeField] GameObject playerLight; 

        public bool canUseSkill = true;

        [SerializeField] ParticleSystem LevelUpEffect;


        void IDamagable.TakeDamage(int amount)
        {
            throw new NotImplementedException();
        }

        public void Move(Queue<Vector3> path)
        {
            StartCoroutine(_RotationCoroutine(path, 0.5f));
        }

        private IEnumerator _RotationCoroutine(Queue<Vector3> path, float rotationDuration)
        {
            Quaternion startRotation = transform.rotation;

            while (true)
            {
                Vector3 targetPos = path.Dequeue();
                if (targetPos == null) break;
                Vector3 direction = targetPos - transform.position;
                Quaternion endRotation = Quaternion.LookRotation(direction, Vector3.up);

                if (Mathf.Approximately(Mathf.Abs(Quaternion.Dot(startRotation, endRotation)), 1.0f) == false)
                {
                    float timeElapsed = 0;
                    while (timeElapsed < rotationDuration)
                    {
                        timeElapsed += Time.deltaTime;
                        float lerpStep = timeElapsed / rotationDuration; // 0-1
                        transform.rotation = Quaternion.Lerp(startRotation, endRotation, lerpStep);
                        yield return null;
                    }
                    transform.rotation = endRotation;
                }
            }
        }

        void ICharacter.Attack(Vector3 targetPos)
        {
            throw new NotImplementedException();
        }

        string ICharacter.GetName()
        {
            return "Player";
        }

        [SerializeField] int _dicePoint;

        public void UsePoint(int usingAmount)
        {
            if (_dicePoint < usingAmount)
            {
                return;
            }
            _dicePoint -= usingAmount;
        }

        public int GetPoint()
        {
            return _dicePoint;
        }

        public void SetPoint(int setValue)
        {
            _dicePoint = setValue;
        }

        private void Awake()
        {
            stat.HP.FillMax();
        }

        protected override void Start()
        {
            base.Start();
            if(!PhotonNetwork.IsMasterClient)
            {
                playerLight.SetActive(false);
            }
        }

        public int Recover(int val)
        {
            if (!stat.IsAlive()) return 0;

            stat.Recover(val);

            //EventChangeHp?.Invoke(this, new IntEventArgs(stat.HP.Value));
            return stat.HP.Value;
        }

        public bool CanAttack()
        {
            return GetPoint() >= _usePointAtAttack;
        }

        protected override void AttackAct(bool isSkill)
        {
            if (isSkill)
            {
                _photonView.RPC("AttackVfx", RpcTarget.All, null);
                return;
            }
            UsePoint(_usePointAtAttack);
        }
        
        public override int GetAttackValue()
        {
            return stat.GetAttackValue() +
                (InventoryManager.Instance.Weapon ? InventoryManager.Instance.Weapon.value : 0);
        }

        [PunRPC]
        public void AttackVfx()
        {
            EffectManager.Instance.SetTarget(gameObject);
            if (InventoryManager.Instance.Weapon)
            {
                int weaponID = InventoryManager.Instance.Weapon.id;
                EffectManager.Instance.ShowImpactVfxHandler(weaponID);
            }
        }

        protected override void DamageAct()
        {
            //EventChangeHp?.Invoke(this, new IntEventArgs(stat.HP.Value));
        }

        public override void DieAct()
        {
            //EventChangeHp?.Invoke(this, new IntEventArgs(stat.HP.Value));
            HexGrid.Instance.GetTileAt(this.transform.position).Entity = null;
            if(PhotonNetwork.IsMasterClient)
            {
                if(GameManager.Instance.remainLife > 0)
                {
                    GameManager.Instance.remainLife -= 1;
                    GameManager.Instance.HealthCountHandler();
                    photonView.RPC("RespawnPlayer", RpcTarget.All, null); 
                }
                else
                {
                    GameManager.Instance.GameEnd(false);
                }
            }
        }

        [PunRPC]
        public void RespawnPlayer()
        {
            Debug.Log("REVIVE");
            gameObject.transform.position = GameManager.Instance.respawnPos.position;
            HexGrid.Instance.GetTileAt(GameManager.Instance.respawnPos.position).Entity = gameObject;
            stat.HP.FillMax();
            Recover(100);
        }

        public void EquipItemHandler(Item item)
        {
            // itemType 0 : helmet
            // itemType 1 : Armor
            // itemType 2 : shield
            
            switch (item.type)
            {
                case Item.ItemType.Helmet:
                    photonView.RPC("EquipItem", RpcTarget.All, 0, item.value); 
                    break;
                case Item.ItemType.Armor:
                    photonView.RPC("EquipItem", RpcTarget.All, 1, item.value); 
                    break;
                case Item.ItemType.Shield:
                    photonView.RPC("EquipItem", RpcTarget.All, 2, item.value); 
                    break;
            }

            //EventEquip?.Invoke(this, null);
        }

        [PunRPC]
        public void EquipItem(int itemType, int value)
        {
            if (!stat.IsAlive()) return;

            switch (itemType)
            {
                case 0:
                    stat.HP.AddMaxValue(value);
                    stat.SetDef(value, true);
                    stat.SetAttackPower(value, true);
                    break;
                case 1:
                    stat.HP.AddMaxValue(value);
                    break;
                case 2:
                    stat.SetDef(value, true);
                    break;
            }
        }
        
        public void UnEquipItemHandler(Item item)
        {
            // itemType 0 : helmet
            // itemType 1 : Armor
            // itemType 2 : shield
            
            switch (item.type)
            {
                case Item.ItemType.Helmet:
                    photonView.RPC("UnequipItem", RpcTarget.All, 0, item.value); 
                    break;
                case Item.ItemType.Armor:
                    photonView.RPC("UnequipItem", RpcTarget.All, 1, item.value); 
                    break;
                case Item.ItemType.Shield:
                    stat.SetDef(item.value, false);
                    break;
            }
            //EventEquip?.Invoke(this, null);
        }
        
        [PunRPC]
        public void UnequipItem(int itemType, int value)
        {
            if (!stat.IsAlive()) return;

            switch (itemType)
            {
                case 0:
                    stat.HP.SubMaxValue(value);
                    stat.SetDef(value, false);
                    stat.SetAttackPower(value, false);
                    break;
                case 1:
                    stat.HP.SubMaxValue(value);
                    break;
                case 2:
                    stat.SetDef(value, false);
                    break;
            }
        }

        public void GetExp(int val)
        {
            if (stat.GetExp(val))
            {
                LevelUpEffect.Play();
                //EventEquip?.Invoke(this, null);
            }
            
        }
    }
}