using EHTool;
using System.Collections.Generic;

public class GameSetting {

    public string Player { get; set; }
    public List<string> Enemy { get; private set; }
    public List<string> BossEnemy { get; private set; }

    public List<string> AllCharacters { get; private set; }

    public GameSetting()
    {
        IDictionaryConnector<string, string> gameData =
            new JsonDictionaryConnector<string, string>();

        IDictionary<string, string> gameDataDict =
            gameData.ReadData("GameData");

        Player = gameDataDict["Player"];

        IDictionaryConnector<string, List<string>> enemyData =
            new JsonDictionaryConnector<string, List<string>>();

        IDictionary<string, List<string>> enemyDataDict =
            enemyData.ReadData("EnemyData");

        Enemy = enemyDataDict["Enemy"];
        BossEnemy = enemyDataDict["BossEnemy"];
        AllCharacters = enemyDataDict["AllCharacters"];


    }

}