using UnityEngine;
using EHTool;
using EHTool.UIKit;

public class LocalGameMaster : MonoBehaviour, IGameMaster {

    [SerializeField] private int _turn;
    private Team[] _teams;

    private EnemySpawner _enemySpawner;

    private IGameMaster.Phase _phase;
    private int _phaseCnt;

    public GameObject InstantiateCharacter(Vector3 position, Quaternion rotation) {
        GameObject retval = AssetOpener.ImportGameObject("LocalCC");

        retval.transform.position = position;
        retval.transform.rotation = rotation;

        return retval;

    }

    public GameObject InstantiateItem(Vector3 position)
    {
        GameObject retval = AssetOpener.ImportGameObject("LocalItem");

        retval.transform.position = position;

        return retval;
    }

    public void StartGame()
    {
        _phase = 0;

        _teams = new Team[2];
        _teams[0] = new Team();
        _teams[1] = new Team();

        GameObject spawner = GameObject.FindWithTag("Spawner");

        _enemySpawner = spawner.GetComponent<EnemySpawner>();
        _enemySpawner.SpawnEnemy();

        for (int i = 0; i < GameManager.Instance.GameSetting.Players.Count; i++) {
            spawner.GetComponent<PlayerSpawner>().SpawnPlayer(i, null);
        }

    }

    public void AddTeamMember(ICharacterController c, int teamIdx)
    {
        _teams[teamIdx].AddMember(c);

        if (_teams[0].GetLeftMemberCount() <
            GameManager.Instance.GameSetting.Players.Count) return;
        if (_phase > 0) return;

        GameStart();

    }

    public void GameStart() {
        _phase = IGameMaster.Phase.Play;
        _teams[0].StartTurn();

    }


    public void RemoveTeamMember(ICharacterController c, int teamIdx)
    {
        _teams[teamIdx].RemoveMember(c);

        if (_teams[teamIdx].GetLeftMemberCount() > 0) return;
        
        if (teamIdx == 0 ) {
            GameEnd(teamIdx != 0);
            return;
        }

        if (_phase == IGameMaster.Phase.Play)
        {

            _phase = IGameMaster.Phase.Boss;
            _enemySpawner.SpawnBoss();

            return;

        }

        _phaseCnt++;

        if (_phaseCnt >= GameManager.Instance.GameSetting.PhaseCnt)
        {
            GameEnd(teamIdx != 0);
            return;
        }

        _phase = IGameMaster.Phase.Play;
        _enemySpawner.SpawnEnemy();

    }

    public void TurnEnd(ICharacterController c)
    {
        _teams[_turn].MemberTurnEnd(c);

        if (!_teams[_turn].CanNextTurn()) return;

        _turn = (_turn + 1) % _teams.Length;
        _teams[_turn].StartTurn();
        
    }

    public void GameEnd(bool victory)
    {
        if (victory) {
            UIManager.Instance.OpenGUI<GUIFullScreen>("GameWin");
            return;
        }
        UIManager.Instance.OpenGUI<GUIFullScreen>("GameOver");
    }

}