using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character {
    public class PlayerController : NetworkCharacterController {

        [SerializeField] int _usePointAtAttack = 3;

        public static event EventHandler<IntEventArgs> EventRecover;
        public static event EventHandler<IntEventArgs> EventDamaged;

        private DicePoint _point;

        private void Awake()
        {
            stat.GetHP().FillMax();
            _point = GetComponent<DicePoint>();
        }

        private void OnEnable()
        {
            Hex curHex = HexGrid.Instance.GetTileAt(this.gameObject.transform.position);
            curHex.Entity = this.gameObject;
            HexGrid.Instance.RemoveAtEmeptyHexTiles(curHex);
        }

        public int Recover(int val)
        {
            if (stat.GetHP().Value <= 0) return 0;

            stat.GetHP().AddValue(val);

            EventRecover?.Invoke(this, new IntEventArgs(stat.GetHP().Value));
            return stat.GetHP().Value;
        }

        public bool CanAttack()
        {
            return _point.GetPoint() >= _usePointAtAttack;
        }

        public override void AttackAct()
        {
            _point.UsePoint(_usePointAtAttack);
            _photonView.RPC("AttackVfx", RpcTarget.All, null);
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

        public override void DamageAct()
        {
            base.DamageAct();
            EventDamaged?.Invoke(this, new IntEventArgs(stat.GetHP().Value));
        }

        public override void DieAct()
        {
            EventDamaged?.Invoke(this, new IntEventArgs(stat.GetHP().Value));
            base.DieAct();
        }
    }
}