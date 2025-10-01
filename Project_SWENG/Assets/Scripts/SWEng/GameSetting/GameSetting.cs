using EasyH;
using System.Collections.Generic;
using System;

namespace SWEng
{

    public class GameSetting : IGameSetting
    {
        public IList<IGameSetting.PlayerSetting> Players { get; set; }
        public IList<string> Enemy { get; set; }
        public IList<string> BossEnemy { get; set; }

        public string Name { get; set; } = "TMP";

        public int MaxPlayerCnt { get; set; } = 3;
        public int EnemyCnt { get; set; } = 1;
        public int PhaseCnt { get; set; } = 1;

        public string MapName { get; set; } = "Local";

        public bool IsMaster { get; set; } = true;

        private ISet<MatchObserver> _observers;

        public IDisposable Subscribe(MatchObserver o)
        {

            if (!_observers.Contains(o))
            {
                _observers.Add(o);

                o.Renewal();
            }

            return new MatchUnsubscriber(_observers, o);
        }

        void Notify()
        {
            foreach (MatchObserver r in _observers)
            {
                r.Renewal();
            }

        }

        public void AddPlayer(string name, string characterCode)
        {
            Notify();
        }

        public void SetPlayer(int idx, string CharacterCode)
        {
            Players[idx].PlayerCharacter = CharacterCode;
            Notify();
        }

        public void RemovePlayer(int idx)
        {
            Notify();

        }

        public void SetPlayerReady(bool v)
        {
            Notify();
        }

        public GameSetting()
        {
            _observers = new HashSet<MatchObserver>();

            IDictionaryConnector<string, List<string>> gameData =
                new JsonDictionaryConnector<string, List<string>>();

            IDictionary<string, List<string>> gameDataDict =
                gameData.ReadData("DefaultGameSettingInfor");

            Players = new List<IGameSetting.PlayerSetting>();

            int i = 0;

            foreach (var data in gameDataDict["Player"])
            {
                Players.Add(new IGameSetting.PlayerSetting
                    (string.Format("Player {0}", i++), data));
            }

            Enemy = gameDataDict["Enemy"];
            BossEnemy = gameDataDict["BossEnemy"];

        }

    }
}