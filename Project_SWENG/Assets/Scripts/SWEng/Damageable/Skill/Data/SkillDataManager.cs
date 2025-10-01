using System.Collections.Generic;
using System.Linq;
using EasyH;

namespace SWEng
{
    public class SkillDataManager : Singleton<SkillDataManager>
    {
        private IDictionary<string, SkillData> _skillDataDict;
        // Start is called before the first frame update

        protected override void OnCreate()
        {

            IDictionaryConnector<string, string> connector
                = new JsonDictionaryConnector<string, string>();

            IDictionary<string, string> rawData = connector.ReadData("SkillInfor");

            _skillDataDict = new Dictionary<string, SkillData>();

            foreach (var data in rawData)
            {
                SkillData skillData =
                    AssetOpener.Import<SkillData>(data.Value);

                _skillDataDict.Add(data.Key, skillData);
            }

        }

        public SkillData GetSkillData(string value)
        {
            if (_skillDataDict.ContainsKey(value))
                return _skillDataDict[value];
            return _skillDataDict.ToList()[0].Value;
        }
    }
}