using Photon.Pun;
using UnityEngine;
using SWEng.Data;
using SWEng.GamePlay;
using SWEng.Network;
using EasyH.UI;
using EasyH.Gaming.TurnBased;

namespace SWEng.MultiPlay.Photon
{
    public class PhotonGameMaster : MonoBehaviourPun, IGameMaster
    {
        private PhotonView _view;

        private int _turn;

        private EnemySpawner _enemySpawner;

        private IGameMaster.Phase _phase;
        private int _phaseCnt;

        public ICharacter InstantiateCharacter(Vector3 position, Quaternion rotation)
        {
            GameObject retval = PhotonNetwork.
                Instantiate("PhotonCC", position, rotation);

            return retval.GetComponent<ICharacter>();

        }

        public GameObject InstantiateItem(Vector3 position)
        {
            GameObject retval = PhotonNetwork.Instantiate("PhotonItem", position, Quaternion.identity);

            return retval;

        }

        public bool StartGame()
        {
            if (!PhotonNetwork.IsMasterClient) return false;

            foreach(var player in GameManager.Instance.Setting.Players)
            {
                if (!player.IsReady && !player.Name.Equals(PhotonNetwork.LocalPlayer.NickName)) return false;
            }

            PhotonNetwork.LoadLevel(
                GameManager.Instance.Setting.MapName);

            return true;
        }
        
        public void DisposeGame()
        {
            NetworkManager.Instance.System.LeaveRoom();
            GameManager.Instance.Setting = new GameSetting();
        }

        public void StartLoad()
        {
            _phase = 0;

            _view = GetComponent<PhotonView>();

            GameObject spawner = GameObject.FindWithTag("Spawner");

            if (PhotonNetwork.IsMasterClient)
            {
                _enemySpawner = spawner.GetComponent<EnemySpawner>();
                _enemySpawner.SpawnEnemy();
            }

            spawner.GetComponent<PlayerSpawner>().
                SpawnPlayer(NetworkManager.Instance.System.PlayerId);

        }

        public void GameStart()
        {
            _phase = IGameMaster.Phase.Play;
            TurnManager.Instance.System.StartGame();

        }

        [PunRPC]
        private void PunAllGameEnd(bool victory)
        {
            GameEnd(victory);
        }

        public void GameEnd(bool victory)
        {
            if (victory)
            {
                UIManager.Instance.OpenGUI<GUIFullScreen>("GameWin");
                return;
            }
            UIManager.Instance.OpenGUI<GUIFullScreen>("GameOver");

        }

    }
}