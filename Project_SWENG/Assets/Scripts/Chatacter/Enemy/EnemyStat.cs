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

        public int speed;
        public int Lv;
        public int Exp;
        public Sprite portrait;
        public bool isDie = false;
        public float percent;
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
            speed = enemyStatus.speed;
            Lv = enemyStatus.Lv;
            portrait = enemyStatus.UIImage;
            Exp = enemyStatus.Exp;
            percent = enemyStatus.percent;
            dropItem = new List<Item>(enemyStatus.dropItem);
        }

    }
}
