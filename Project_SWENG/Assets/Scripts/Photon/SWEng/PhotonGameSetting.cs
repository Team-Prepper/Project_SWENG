using EasyH;
using Photon.Pun;
using System.Collections.Generic;
using System;
using UnityEngine;
using SWEng;

namespace MultiPlay.Photon.SWEng
{
    [RequireComponent(typeof(PhotonView))]
    public class PhotonGameSetting : MonoBehaviourPun, IPunObservable, IGameSetting
    {

        public string Name =>
            PhotonNetwork.CurrentRoom.Name;
        public int MaxPlayerCnt =>
            PhotonNetwork.CurrentRoom.MaxPlayers;

        public int EnemyCnt { get; set; } = 1;
        public int PhaseCnt { get; set; } = 1;

        public string MapName { get; set; } = "Local";

        public IList<IGameSetting.PlayerSetting> Players { get; set; }
        public IList<string> Enemy { get; set; }
        public IList<string> BossEnemy { get; set; }

        public bool IsMaster => PhotonNetwork.IsMasterClient;

        private PhotonView _view;
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

        public void SetPlayer(int idx, string CharacterCode)
        {
            if (!PhotonNetwork.LocalPlayer.NickName.
                Equals(Players[idx].Name)) return;

            _view.RPC(nameof(PunServerSetPlayer),
                RpcTarget.MasterClient, idx, CharacterCode);
        }

        [PunRPC]
        private void PunServerSetPlayer(int idx, string CharacterCode)
        {
            Players[idx].PlayerCharacter = CharacterCode;
            Notify();
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
            Players.Add(new IGameSetting.PlayerSetting(name, characterCode));
            Notify();
        }

        public void RemovePlayer(int idx)
        {
            Players.RemoveAt(idx);
        }

        public void Awake()
        {
            _view = GetComponent<PhotonView>();
            _observers = new HashSet<MatchObserver>();

            IDictionaryConnector<string, List<string>> gameData =
                new JsonDictionaryConnector<string, List<string>>();

            IDictionary<string, List<string>> gameDataDict =
                gameData.ReadData("DefaultGameSettingInfor");

            Players = new List<IGameSetting.PlayerSetting>();

            Enemy = gameDataDict["Enemy"];
            BossEnemy = gameDataDict["BossEnemy"];

            _view.FindObservables(true);

        }

        private int FindIdxByName(string name)
        {
            for (int i = 0; i < Players.Count; i++)
            {
                if (name.Equals(Players[i].Name)) return i;
            }

            return 0;
        }

        public void SetPlayerReady(bool v)
        {
            Debug.Log(PhotonNetwork.LocalPlayer.NickName);

            _view.RPC(nameof(ReadyToServer), RpcTarget.MasterClient,
                PhotonNetwork.LocalPlayer.NickName, v ? 1 : 0);
        }

        [PunRPC]
        private void ReadyToServer(string name, int value)
        {
            Players[FindIdxByName(name)].IsReady = value == 1;
        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {

            if (stream.IsWriting)
            {
                stream.SendNext(Players.Count);
                for (int i = 0; i < Players.Count; i++)
                {
                    stream.SendNext(Players[i].Name);
                    stream.SendNext(Players[i].PlayerCharacter);
                    stream.SendNext(Players[i].IsReady);
                }

                Notify();
                return;
            }

            int cnt = (int)stream.ReceiveNext();
            Players = new List<IGameSetting.PlayerSetting>();

            for (int i = 0; i < cnt; i++)
            {
                string name = (string)stream.ReceiveNext();
                string cc = (string)stream.ReceiveNext();
                bool isReady = (bool)stream.ReceiveNext();
                Players.Add(new IGameSetting.PlayerSetting(name, cc, isReady));
            }

            Notify();
        }

    }
}