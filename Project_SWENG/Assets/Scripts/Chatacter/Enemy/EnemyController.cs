using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Serialization;

namespace Character {
    public class EnemyController : NetworkCharacterController {

        [SerializeField] GUI_EnemyHealth healthGUI;
        [SerializeField] LayerMask playerLayerMask;
        public EnemySpawner enemySpawner;
        
        public EnemyStat enemyStat;

        public Hex curHex;

        public override string GetName()
        {
            return enemyStat.monsterName;
        }
        private void OnEnable()
        {
            Debug.Log("Enemy OnEnable");
            if (enemyStat == null)
                enemyStat = GetComponent<EnemyStat>();

            if (healthGUI == null)
                healthGUI = GetComponentInChildren<GUI_EnemyHealth>();

            //stat.HP = new GaugeValue<int>(enemyStat.maxHp, enemyStat.maxHp, 0);
            stat.SetHP(enemyStat.maxHp, enemyStat.maxHp, 0);
            curHex = HexGrid.Instance.GetTileAt(this.gameObject.transform.position);

            curHex.Entity = this.gameObject;
            
        }

        public override int GetAttackValue()
        {
            Debug.Log(enemyStat.atk);
            return enemyStat.atk;
        }

        public override void DamageAct()
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, 10f, playerLayerMask);
            if (colliders.Length > 0)
                gameObject.transform.LookAt(colliders[0].transform);

            healthGUI.UpdateGUI((float)stat.HP.Value / enemyStat.maxHp);
        }

        public override void DieAct()
        {
            curHex = HexGrid.Instance.GetTileAt(this.gameObject.transform.position);
            curHex.Entity = null;
            if(PhotonNetwork.IsMasterClient)
                enemySpawner.enemyList.Remove(this.gameObject);

            healthGUI.UpdateGUI(0);
            DropItem();
            Destroy(this.gameObject, 1f);
        }

        private void DropItem()
        {
            if (enemyStat.dropItem.Count == 0) return;
            Item dropitem = enemyStat.dropItem[Random.Range(0, enemyStat.dropItem.Count)];
            curHex.Item = dropitem;
            dropitem.itemHex = this.curHex;
        }
    }
}
