using System.Collections.Generic;
using System;

public interface MatchObserver {
    public void Renewal();
}

public class MatchUnsubscriber : IDisposable {
    private readonly ISet<MatchObserver> _observers;
    private readonly MatchObserver _observer;

    public MatchUnsubscriber(ISet<MatchObserver> observers, MatchObserver observer)
    {
        _observers = observers;
        _observer = observer;
    }

    public void Dispose()
    {
        if (_observers.Contains(_observer))
            _observers.Remove(_observer);
    }
}

public interface IGameSetting
{
    [Serializable]
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

    public bool IsMaster { get; }

    public string Name { get; }
    public int MaxPlayerCnt { get; }
    public int EnemyCnt { get; set; }
    public int PhaseCnt { get; set; }

    public string MapName { get; set; }
    
    public IDisposable Subscribe(MatchObserver observer);

    public IList<PlayerSetting> Players { get; }
    public IList<string> Enemy { get; set; }
    public IList<string> BossEnemy { get; set; }


    public bool StartGame();
    public void DisposeGame();

    public void SetPlayer(int idx, string characterCode);
    public void SetPlayerReady(bool v);
    public void AddPlayer(string name, string characterCode);
    public void RemovePlayer(int idx);

}