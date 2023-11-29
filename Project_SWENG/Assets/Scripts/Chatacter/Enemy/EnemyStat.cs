using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character {
    [RequireComponent(typeof(EnemyAttack)), RequireComponent(typeof(EnemyController))]
    public class EnemyStat : MonoBehaviour {
        [Header("Enemy stat")]
        [SerializeField] EnemyStatus enemyStatus;
        public string monsterName;
        public int maxHp;
        public int atk;
        public int def;

        public int Lv;
        public int Exp;
        public bool isDie = false;
        public bool isBoss = false;
        public List<Item> dropItem;

        private void Awake()
        {
            SetEnemyStat();
        }

        public void SetEnemyStat()
        {
            monsterName = enemyStatus.monsterName;
            maxHp = enemyStatus.maxHp;
            atk = enemyStatus.atk;
            def = enemyStatus.def;
            Lv = enemyStatus.Lv;
            Exp = enemyStatus.Exp;
            isBoss = enemyStatus.isBoss;
            dropItem = new List<Item>(enemyStatus.dropItem);
        }

    }
}
