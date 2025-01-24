using UnityEngine;
using EHTool;

public class LocalGameMaster : MonoBehaviour, IGameMaster {

    public GameSetting Setting { get; set; }

    public IGameMaster.Phase _gamePhase;

    [SerializeField] int _turn;
    Team[] _teams;

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
        if (Setting == null)
            Setting = new GameSetting();

        _teams = new Team[2];
        _teams[0] = new Team();
        _teams[1] = new Team();

        GameObject spawner = GameObject.FindWithTag("Spawner");

        spawner.GetComponent<EnemySpawner>().SpawnEnemy();
        spawner.GetComponent<PlayerSpawner>().SpawnPlayer(
            GameManager.Instance.Network.PlayerId);

        _gamePhase = IGameMaster.Phase.Play;
        _teams[0].StartTurn();

    }

    public void AddTeamMember(ICharacterController c, int teamIdx)
    {
        _teams[teamIdx].AddMember(c);

    }

    public void RemoveTeamMember(ICharacterController c, int teamIdx)
    {
        _teams[teamIdx].RemoveMember(c);

    }

    public void TurnEnd(ICharacterController c)
    {
        _teams[_turn].MemberTurnEnd(c);

        if (!_teams[_turn].CanNextTurn()) return;

        _turn = (_turn + 1) % _teams.Length;
        _teams[_turn].StartTurn();

    }

    void changeDayNight()
    {
        return;
        /*
        if (gamePhase != IGameMaster.Phase.EnemyPhase)
        {
            day.SetActive(true);
            night.SetActive(false);
        }
        else
        {
            day.SetActive(false);
            night.SetActive(true);
        }*/
    }

    public void GameEnd(bool victory)
    {
        _gamePhase = IGameMaster.Phase.End;
    }

}