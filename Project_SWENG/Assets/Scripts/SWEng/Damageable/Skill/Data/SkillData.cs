using UnityEngine;

namespace SWEng {

    [CreateAssetMenu(fileName = "Skill",
        menuName = "Custom/SkillData", order = 2)]
    public class SkillData : ScriptableObject
    {

        public string SkillName;
        public AnimationClip AnimClip;
        public string SkillValue = "RangeSkill/5";

    }
}