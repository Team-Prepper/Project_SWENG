using Photon.Pun;
using UnityEngine;
using EHTool.UIKit;

public class PhotonGameMaster : MonoBehaviourPun, IGameMaster
{
    private PhotonView _view;

    private int _turn;
    private Team[] _teams;

    private EnemySpawner _enemySpawner;
    private int _phase;

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
        _phase = 0;

        _view = GetComponent<PhotonView>();

        GameObject spawner = GameObject.FindWithTag("Spawner");

        if (PhotonNetwork.IsMasterClient)
        {
            _teams = new Team[2];
            _teams[0] = new Team();
            _teams[1] = new Team();
            _enemySpawner = spawner.GetComponent<EnemySpawner>();
            _enemySpawner.SpawnEnemy();
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
        if (_phase > 0) return;

        GameStart();
        
    }

    public void GameStart() {
        _phase = 1;
        _teams[0].StartTurn();

    }

    public void RemoveTeamMember(ICharacterController c, int teamIdx)
    {
        _teams[teamIdx].RemoveMember(c);

        if (_teams[teamIdx].GetLeftMemberCount() > 0) return;

        if (teamIdx != 0 && _phase == 1) {
            _phase++;
            _enemySpawner.SpawnBoss();
            return;
        }

        _view.RPC("PunAllGameEnd", RpcTarget.All, teamIdx == 0);
    }

    [PunRPC]
    private void PunAllGameEnd(bool victory) {
        GameEnd(victory);
    }

    public void GameEnd(bool victory)
    {
        if (victory) {
            UIManager.Instance.OpenGUI<GUIFullScreen>("GameWin");
            return;
        }
        UIManager.Instance.OpenGUI<GUIFullScreen>("GameOver");

    }

    public void TurnEnd(ICharacterController c)
    {
        _teams[_turn].MemberTurnEnd(c);

        if (!_teams[_turn].CanNextTurn()) return;

        _turn = (_turn + 1) % _teams.Length;
        _teams[_turn].StartTurn();
    }

}