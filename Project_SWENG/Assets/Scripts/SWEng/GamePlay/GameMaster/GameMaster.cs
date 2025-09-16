using UnityEngine;
using EasyH;
using EasyH.UI;
using EasyH.Gaming.TurnBased;
using UnityEngine.SceneManagement;
using SWEng.Data;

namespace SWEng.GamePlay
{
    public class GameMaster : MonoBehaviour, IGameMaster
    {

        [SerializeField] private int _turn;

        private EnemySpawner _enemySpawner;

        private IGameMaster.Phase _phase;
        private int _phaseCnt;

        public ICharacter InstantiateCharacter(Vector3 position, Quaternion rotation)
        {
            ICharacter retval = AssetOpener.
                ImportComponent<Character>("LocalCC");

            retval.transform.
                SetPositionAndRotation(position, rotation);
                
            return retval;

        }

        public GameObject InstantiateItem(Vector3 position)
        {
            GameObject retval = AssetOpener.ImportGameObject("LocalItem");

            retval.transform.position = position;

            return retval;
        }

        public bool StartGame()
        {
            SceneManager.LoadSceneAsync(
                GameManager.Instance.Setting.MapName);
            //UIManager.Instance.OpenGUI<GUI_Loading>("Loading");

            return true;

        }

        public void DisposeGame()
        {
            
        }

        public void StartLoad()
        {
            _phase = 0;

            TurnManager.Instance.System = new TurnSystem();

            GameObject spawner = GameObject.FindWithTag("Spawner");

            _enemySpawner = spawner.GetComponent<EnemySpawner>();
            _enemySpawner.SpawnEnemy();

            for (int i = 0; i < GameManager.Instance.Setting.Players.Count; i++)
            {
                spawner.GetComponent<PlayerSpawner>().SpawnPlayer(i, null);
            }

            _phase = IGameMaster.Phase.Play;

            TurnManager.Instance.System.StartGame();
            //TurnManager.Instance.System.Subscribe();

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