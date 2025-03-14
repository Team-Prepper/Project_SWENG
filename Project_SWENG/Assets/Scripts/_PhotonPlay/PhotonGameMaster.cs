using Photon.Pun;
using UnityEngine;

public class PhotonGameMaster : MonoBehaviourPun, IGameMaster
{

    private IGameMaster.Phase _gamePhase;

    private Team[] _teams;
    private int _turn;

    public GameObject InstantiateCharacter(Vector3 position, Quaternion rotation)
    {
        GameObject retval = PhotonNetwork.Instantiate("PhotonCC", position, rotation);

        return retval;

    }

    public GameObject InstantiateItem(Vector3 position)
    {
        GameObject retval = PhotonNetwork.Instantiate("PhotonItem", position, Quaternion.identity);

        return retval;

    }

    public void StartGame()
    {
        GameObject spawner = GameObject.FindWithTag("Spawner");

        if (PhotonNetwork.IsMasterClient)
        {
            _teams = new Team[2];
            _teams[0] = new Team();
            _teams[1] = new Team();
            spawner.GetComponent<EnemySpawner>().SpawnEnemy();
        }

        spawner.GetComponent<PlayerSpawner>().SpawnPlayer(
            GameManager.Instance.Network.PlayerId);

    }

    public void AddTeamMember(ICharacterController c, int teamIdx)
    {
        _teams[teamIdx].AddMember(c);

        if (!PhotonNetwork.IsMasterClient) return;
        if (_teams[0].GetLeftMemberCount() <
            GameManager.Instance.GameSetting.Players.Count) return;
            
        _gamePhase = IGameMaster.Phase.Play;
        _teams[0].StartTurn();
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
        _gamePhase = IGameMaster.Phase.End;
    }

}