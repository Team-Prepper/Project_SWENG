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

        protected override void DamageAct()
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
            if (PhotonNetwork.IsMasterClient)
            {
                GameManager.Instance.enemies.Remove(this.gameObject);
                if (enemyStat.isBoss)
                    GameManager.Instance.bossEnemies.Remove(gameObject);
            }
            healthGUI.UpdateGUI(0);
            DropItem();
            DropExp();
            Destroy(this.gameObject, 1f);
        }

        private void DropItem()
        {
            if (enemyStat.dropItem.Count == 0) return;
            Item dropitem = enemyStat.dropItem[Random.Range(0, enemyStat.dropItem.Count)];
            curHex.Item = dropitem;
            dropitem.itemHex = this.curHex;
        }

        private void DropExp()
        {
            foreach (var neighbours in HexGrid.Instance.GetNeighboursFor(curHex.HexCoords))
            {
                Hex curHex = HexGrid.Instance.GetTileAt(neighbours);
                GameObject entity = curHex.Entity;
                if (entity != null && entity.CompareTag("Player"))
                {
                    entity.GetComponent<PlayerController>()?.GetExp(enemyStat.Exp);
                }
            }
        }
    }
}
