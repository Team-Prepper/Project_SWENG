using UnityEngine;
using EHTool;

public class LocalGameMaster : MonoBehaviour, IGameMaster {

    Team[] _teams;
    [SerializeField] int _turn;

    [SerializeField] private PlayerSpawner _playerSpawner;
    [SerializeField] private EnemySpawner _enemySpawner;

    public IGameMaster.Phase gamePhase;

    public GameObject InstantiateCharacter(Vector3 position, Quaternion rotation) {
        GameObject retval = AssetOpener.ImportGameObject("LocalCC");

        retval.transform.position = position;
        retval.transform.rotation = rotation;

        return retval;

    }

    public GameObject InstantiateItem(Vector3 position) {
        return null;
    }

    private void Start()
    {
        GameManager.Instance.SetGameMaster(this);
        _teams = new Team[2];
        _teams[0] = new Team();
        _teams[1] = new Team();

        _playerSpawner.SpawnPlayer(0);
        _enemySpawner.SpawnEnemy();

        gamePhase = IGameMaster.Phase.Play;
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

    private void Update()
    {
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
        gamePhase = IGameMaster.Phase.End;
    }

}