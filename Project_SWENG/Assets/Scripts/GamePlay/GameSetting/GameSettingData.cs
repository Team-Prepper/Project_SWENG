using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameSetting", menuName = "Custom/GameSettingData", order = 2)]
public class GameSettingData : ScriptableObject
{
    public string Name;
    public string MapName;

    public int PhaseEnemyCnt = 3;
    public int PhaseCnt = 2;

    public List<string> PlayerList;
    public List<string> EnemyList;
    public List<string> BossEnemyList;

}