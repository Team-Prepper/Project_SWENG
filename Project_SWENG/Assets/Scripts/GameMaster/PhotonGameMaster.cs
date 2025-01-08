using Photon.Pun;
using UnityEngine;

public class PhotonGameMaster : MonoBehaviourPun, IGameMaster {

    Team[] _teams;
    int _turn;

    [SerializeField] private PlayerSpawner _playerSpawner;
    [SerializeField] private EnemySpawner _enemySpawner;

    public IGameMaster.Phase gamePhase;

    public GameObject InstantiateCharacter(Vector3 position, Quaternion rotation)
    {
        GameObject retval = PhotonNetwork.Instantiate("PhotonCC", position, rotation);

        return retval;

    }

    public GameObject InstantiateItem(Vector3 position)
    {
        return null;
    }

    private void Start()
    {
        GameManager.Instance.SetGameMaster(this);

        if (PhotonNetwork.IsMasterClient)
        {
            _teams = new Team[2];
            _teams[0] = new Team();
            _teams[1] = new Team();
            _enemySpawner.SpawnEnemy();
        }

        _playerSpawner.SpawnPlayer(
            GameManager.Instance.Network.PlayerId);
        
        if (PhotonNetwork.IsMasterClient)
        {
            gamePhase = IGameMaster.Phase.Play;
            _teams[0].StartTurn();
        }
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

    public void GameEnd(bool victory)
    {
        gamePhase = IGameMaster.Phase.End;
    }

}