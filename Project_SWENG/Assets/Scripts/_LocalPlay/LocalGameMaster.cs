#nullable disable

using UnityEngine;
using EHTool;
using EHTool.UIKit;

public class LocalGameMaster : MonoBehaviour, IGameMaster {

    [SerializeField] private int _turn;
    private Team[] _teams;

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

        _teams = new Team[2];
        _teams[0] = new Team();
        _teams[1] = new Team();

        GameObject spawner = GameObject.FindWithTag("Spawner");

        spawner.GetComponent<EnemySpawner>().SpawnEnemy();

        for (int i = 0; i < GameManager.Instance.GameSetting.Players.Count; i++) {
            spawner.GetComponent<PlayerSpawner>().SpawnPlayer(i, null);
        }

    }

    public void AddTeamMember(ICharacterController c, int teamIdx)
    {
        _teams[teamIdx].AddMember(c);

        if (_teams[0].GetLeftMemberCount() <
            GameManager.Instance.GameSetting.Players.Count) return;

        _teams[0].StartTurn();

    }

    public void RemoveTeamMember(ICharacterController c, int teamIdx)
    {
        _teams[teamIdx].RemoveMember(c);

        if (_teams[teamIdx].GetLeftMemberCount() > 0) return;
        GameEnd(teamIdx == 0);

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
            UIManager.Instance.OpenGUI<GUIFullScreen>("");
            return;
        }
        UIManager.Instance.OpenGUI<GUIFullScreen>("GameOver");
    }

}