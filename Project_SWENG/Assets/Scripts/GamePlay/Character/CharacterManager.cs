using EHTool;
using System.Collections.Generic;
using System.Linq;

public class CharacterManager : Singleton<CharacterManager>
{
    private IDictionary<string, CharacterData> _characterDataDict;

    protected override void OnCreate() {

        IDictionaryConnector<string, string> connector
            = new JsonDictionaryConnector<string, string>();

        IDictionary<string, string> rawData = connector.ReadData("CharacterInfor");

        _characterDataDict = new Dictionary<string, CharacterData>();

        foreach(var data in rawData) {
            _characterDataDict.Add(data.Key, AssetOpener.Import<CharacterData>(data.Value));
        }

    }

    public IList<string> AllCharacters => _characterDataDict.Keys.ToList();

    public CharacterData GetCharacterData(string value) {
        if (_characterDataDict.ContainsKey(value))
            return _characterDataDict[value];
        return _characterDataDict.ToList()[0].Value;
    }

}
