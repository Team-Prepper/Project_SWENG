using UnityEngine;
using EasyH;
using EasyH.Unity;
using EasyH.Unity.UI;
using EasyH.Gaming.TurnBased;
using UnityEngine.SceneManagement;

namespace SWEng
{
    public class GameMaster : MonoBehaviour, IGameMaster
    {

        [SerializeField] private int _turn;

        private EnemySpawner _enemySpawner;

        private IGameMaster.Phase _phase;
        private int _phaseCnt;

        public ICharacter InstantiateCharacter(Vector3 position, Quaternion rotation)
        {
            ICharacter retval = ResourceManager.Instance.
                ResourceConnector.ImportComponent<Character>("LocalCC");

            retval.transform.
                SetPositionAndRotation(position, rotation);
                
            return retval;

        }

        public GameObject InstantiateItem(Vector3 position)
        {
            GameObject retval = ResourceManager.Instance.
                ResourceConnector.ImportGameObject("LocalItem");

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

            IGameSetting setting =
                GameManager.Instance.Setting;

            ITurnSystem sys = new TurnSystem();
            TurnManager.Instance.System = sys;

            sys.SetStartCondition(() =>
            {
                Debug.Log("Check");
                Debug.Log(sys.GetTeamMemberCnt(0));
                if (sys.GetTeamMemberCnt(0)
                    >= setting.Players.Count)
                {
                    return true;
                }
                return false; 
            });

            GameObject spawner = GameObject.FindWithTag("Spawner");

            _enemySpawner = spawner.GetComponent<EnemySpawner>();
            _enemySpawner.SpawnEnemy();

            for (int i = 0; i < setting.Players.Count; i++)
            {
                spawner.GetComponent<PlayerSpawner>().SpawnPlayer(i, null);
            }

            _phase = IGameMaster.Phase.Play;
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