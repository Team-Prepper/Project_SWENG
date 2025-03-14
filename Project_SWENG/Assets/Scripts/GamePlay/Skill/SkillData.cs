using UnityEngine;

[CreateAssetMenu(fileName = "Skill", menuName = "Custom/SkillData", order = 2)]
public class SkillData : ScriptableObject {

    public string SkillName;
    public string AnimName;

    public string SkillValue = "BasicSkill/5";

    public ISkill GetSkill() {
        string[] parsed = SkillValue.Split("/");

        switch (parsed[0]) {
            case "RangeSkill":
                return new BasicSkill(int.Parse(parsed[1]));
            case "TargetingSkill":
                return new BasicTargetingSkill(int.Parse(parsed[1]));
        }
        
        return new BasicSkill(3);
    }

}
