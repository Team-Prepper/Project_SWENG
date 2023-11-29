using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyStatus", menuName = "EnemyStatus/EnemyBaseStatsData", order = 2)]

public class EnemyStatus : ScriptableObject
{
    public string monsterName = "Normal";
    public int atk = 10;
    public int def = 10;
    public int maxHp = 60;
    
    public int Lv = 1;
    public int Exp = 10;

    public bool isBoss = false;
    public Sprite UIImage; 
    public List<Item> dropItem;
}
