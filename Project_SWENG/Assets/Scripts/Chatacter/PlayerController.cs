using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character {
    public class PlayerController : NetworkCharacterController {

        [SerializeField] int atkPoint = 3;

        public static event EventHandler<IntEventArgs> EventRecover;
        public static event EventHandler<IntEventArgs> EventDamaged;

        private DicePoint unit;

        private void Awake()
        {
            stat.HP.FillMax();
            unit = GetComponent<DicePoint>();
            _PhotonView = GetComponent<PhotonView>();
        }

        private void OnEnable()
        {
            Hex curHex = HexGrid.Instance.GetHexFromPosition(this.gameObject.transform.position);
            curHex.Entity = this.gameObject;
            HexGrid.Instance.RemoveAtEmeptyHexTiles(curHex);
        }

        public int Recover(int val)
        {
            if (stat.HP.Value <= 0) return 0;

            stat.HP.AddValue(val);

            EventRecover?.Invoke(this, new IntEventArgs(stat.HP.Value));
            return stat.HP.Value;
        }

        public bool CanAttack()
        {
            return unit.GetPoint() >= atkPoint;
        }

        public override void AttackAct()
        {
            unit.UsePoint(atkPoint);
            _PhotonView.RPC("AttackVfx", RpcTarget.All, null);
        }

        public override int GetAttackValue()
        {
            return stat.attackPower +
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

        public override void DamageAct()
        {
            base.DamageAct();
            EventDamaged?.Invoke(this, new IntEventArgs(stat.HP.Value));
        }

        public override void DieAct()
        {
            EventDamaged?.Invoke(this, new IntEventArgs(stat.HP.Value));
            base.DieAct();
        }
    }
}