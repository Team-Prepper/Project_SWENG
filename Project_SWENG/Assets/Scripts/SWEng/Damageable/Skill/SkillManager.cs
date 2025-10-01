using EasyH;

namespace SWEng
{
    public class SkillManager : Singleton<SkillManager>
    {

        public ISkill GetSkill(string value)
        {
            SkillData data = SkillDataManager.Instance.GetSkillData(value);

            string[] parsed = data.SkillValue.Split("/");

            switch (parsed[0])
            {
                case "RangeSkill":
                    return new BasicSkill(int.Parse(parsed[1]));
                case "TargetingSkill":
                    return new BasicTargetingSkill
                        (int.Parse(parsed[1]));
            }

            return new BasicSkill(3);
        }

    }
    
}