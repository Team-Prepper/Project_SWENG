using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Character {

    public class PlayerController : NetworkCharacterController {

        [SerializeField] int _usePointAtAttack = 3;
        [SerializeField] GameObject playerLight; 
        public static event EventHandler<IntEventArgs> EventChangeHp;
        public static event EventHandler<EventArgs> EventEquip;
        private DicePoint _point;

        public bool canUseSkill = true;

        [SerializeField] ParticleSystem LevelUpEffect;

        private void Awake()
        {
            stat.HP.FillMax();
            _point = GetComponent<DicePoint>();
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

            EventChangeHp?.Invoke(this, new IntEventArgs(stat.HP.Value));
            return stat.HP.Value;
        }

        public bool CanAttack()
        {
            return _point.GetPoint() >= _usePointAtAttack;
        }

        protected override void AttackAct(bool isSkill)
        {
            if (isSkill)
            {
                _photonView.RPC("AttackVfx", RpcTarget.All, null);
                return;
            }
            _point.UsePoint(_usePointAtAttack);
        }

        public override string GetName()
        {
            return "Player";
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
            EventChangeHp?.Invoke(this, new IntEventArgs(stat.HP.Value));
        }

        public override void DieAct()
        {
            EventChangeHp?.Invoke(this, new IntEventArgs(stat.HP.Value));
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

            EventEquip?.Invoke(this, null);
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
                EventEquip?.Invoke(this, null);
            }
            
        }
    }
}