using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Skill", menuName = "Custom/SkillData", order = 2)]
public class SkillData : ScriptableObject {

    public string SkillName;

    public bool IsRangeAttack;
    public int UsePoint;

    public string AnimName;
}
