using CharacterSystem;
using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UISystem;
using UnityEngine;
using UnityEngine.UI;

public class NetworkGameMaster : MonoBehaviourPun, IGameMaster {

    Team[] _teams;
    int _turn;

    [SerializeField] private PlayerSpawner _playerSpawner;
    [SerializeField] private EnemySpawner _enemySpawner;

    public IGameMaster.Phase gamePhase;

    public GameObject InstantiateCharacter(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        GameObject retval = Instantiate(prefab, position, rotation);
        retval.AddComponent<LocalCharacterController>().Initial();

        return retval;

    }

    private void Start()
    {
        GameManager.Instance.SetGameMaster(this);
        _teams = new Team[1];
        _teams[0] = new Team();

        _playerSpawner.SpawnPlayer(NetworkManager.PlayerID);

        gamePhase = IGameMaster.Phase.Play;
        _teams[0].StartTurn();
    }

    public void AddPlayerTeamMember(ICharacterController c)
    {
        _teams[0].AddMember(c);

    }
    public void AddEnemyTeamMember(ICharacterController c)
    {
        _teams[1].AddMember(c);

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