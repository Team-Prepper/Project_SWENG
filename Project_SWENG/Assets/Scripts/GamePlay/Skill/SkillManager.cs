using EHTool;
using System.Collections.Generic;
using System.Linq;

public class SkillManager : Singleton<SkillManager>
{
    private IDictionary<string, SkillData> _skillDataDict;
    
    protected override void OnCreate() {

        IDictionaryConnector<string, string> connector
            = new JsonDictionaryConnector<string, string>();

        IDictionary<string, string> rawData = connector.ReadData("SkillInfor");

        _skillDataDict = new Dictionary<string, SkillData>();

        foreach(var data in rawData) {
            _skillDataDict.Add(data.Key, AssetOpener.Import<SkillData>(data.Value));
        }

    }

    public SkillData GetSkillData(string value) {
        if (_skillDataDict.ContainsKey(value))
            return _skillDataDict[value];
        return _skillDataDict.ToList()[0].Value;
    }

}
