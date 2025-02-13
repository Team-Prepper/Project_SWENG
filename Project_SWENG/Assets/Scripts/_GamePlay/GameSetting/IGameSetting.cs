using System.Collections.Generic;

public interface IGameSetting
{
    [System.Serializable]
    public class PlayerSetting {
        public string Name;
        public string PlayerCharacter;
        public bool IsReady;

        public PlayerSetting(string name, string cc, bool isReady = false) {
            Name = name;
            PlayerCharacter = cc;
            IsReady = isReady;
        }
    }

    public string Name { get; }
    public int MaxPlayerCnt { get; }

    public IList<PlayerSetting> Players { get; }
    public IList<string> Enemy { get; }
    public IList<string> BossEnemy { get; }
    public IList<string> AllCharacters { get; }

    public void SetPlayer(int idx, string characterCode);

    public void AddEnemy(string characterCode);
    public void RemoveEnemy(string characterCode);
    public void AddBossEnemy(string characterCode);
    public void RemoveBossEnemy(string characterCode);
    void SetPlayerReady(bool v);
}