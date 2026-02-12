using Photon.Pun;
using UnityEngine;
using SWEng.Network;
using EasyH.Unity.UI;
using EasyH.Gaming.TurnBased;
using SWEng;

namespace MultiPlay.Photon.SWEng
{
    public class PhotonGameMaster : MonoBehaviourPun, IGameMaster
    {
        private PhotonView _view;

        private EnemySpawner _enemySpawner;

        private IGameMaster.Phase _phase;

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
            
            ITurnSystem sys = new TurnSystem();
            TurnManager.Instance.System = sys;

            sys.SetStartCondition(() =>
            {
                if (!PhotonNetwork.IsMasterClient)
                    return false;

                if (sys.GetTeamMemberCnt(0)
                    >= GameManager.Instance.Setting.Players.Count)
                {
                    return true;
                }
                return false; 
            });

            GameObject spawner = GameObject.FindWithTag("Spawner");

            if (PhotonNetwork.IsMasterClient)
            {
                _enemySpawner = spawner.GetComponent<EnemySpawner>();
                _enemySpawner.SpawnEnemy();
            }

            spawner.GetComponent<PlayerSpawner>().
                SpawnPlayer(NetworkManager.Instance.System.PlayerId);

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