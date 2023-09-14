using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStat : MonoBehaviour
{
    [Header("Enemy stat")]
    [SerializeField] EnemyStatus enemyStatus;
    public string monsterName;
    public int maxHp;
    public int curHp;
    public int atk;
    public int def;

    public int speed;
    public int Lv;
    public int Exp;
    public int maxSlot;
    public Sprite portrait;
    public bool isDie = false;
    public string attackType;
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
        curHp = enemyStatus.maxHp;
        atk = enemyStatus.atk;
        def = enemyStatus.def;
        speed = enemyStatus.speed;
        Lv = enemyStatus.Lv;
        portrait = enemyStatus.UIImage;
        Exp = enemyStatus.Exp;
        maxSlot = enemyStatus.maxSlot;
        attackType = enemyStatus.AttackType;
        percent = enemyStatus.percent;
        dropItem = new List<Item>(enemyStatus.dropItem);
    }

}
