using EHTool;
using System.Collections.Generic;

public class LocalGameSetting : IGameSetting {

    public IList<IGameSetting.PlayerSetting> Players { get; set; }
    public IList<string> Enemy { get; private set; }
    public IList<string> BossEnemy { get; private set; }
    public IList<string> AllCharacters { get; private set; }

    public string Name => "TMP";

    public int MaxPlayerCnt => 3;

    public void SetPlayer(int idx, string CharacterCode)
    {
        Players[idx].PlayerCharacter = CharacterCode;
    }

    public void AddEnemy(string characterCode) { 
        Enemy.Add(characterCode);
    }

    public void RemoveEnemy(string characterCode) {
        Enemy.Remove(characterCode);
    }

    public void AddBossEnemy(string characterCode)
    {
        BossEnemy.Add(characterCode);

    }

    public void RemoveBossEnemy(string characterCode)
    {
        BossEnemy.Remove(characterCode);
    }

    public void SetPlayerReady(bool v)
    {
        throw new System.NotImplementedException();
    }

    public LocalGameSetting()
    {
        IDictionaryConnector<string, string> gameData =
            new JsonDictionaryConnector<string, string>();

        IDictionary<string, string> gameDataDict =
            gameData.ReadData("GameData");

        Players = new List<IGameSetting.PlayerSetting>
        {
            new IGameSetting.PlayerSetting("Player", gameDataDict["Player"])
        };

        IDictionaryConnector<string, List<string>> enemyData =
            new JsonDictionaryConnector<string, List<string>>();

        IDictionary<string, List<string>> enemyDataDict =
            enemyData.ReadData("EnemyData");

        Enemy = enemyDataDict["Enemy"];
        BossEnemy = enemyDataDict["BossEnemy"];
        AllCharacters = enemyDataDict["AllCharacters"];

    }

}