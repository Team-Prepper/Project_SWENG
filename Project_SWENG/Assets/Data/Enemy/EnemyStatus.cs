using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyStatus", menuName = "ScriptableObjects/EnemyBaseStatsData", order = 2)]

public class EnemyStatus : ScriptableObject
{
   
    public string monsterName = "Normal";
    public float atk = 10f;
    public float def = 10f;
    public float maxHp = 60f;
    public float nowHp = 60f;
    public float percent = 70;
   
    //속도 
    public int speed = 50;
    
    public int Lv = 1;
    public int Exp = 10;
    public int maxSlot = 3;
    public Sprite UIImage; //초상화 이미지
    public string AttackType = "attackBlackSmith";
}
