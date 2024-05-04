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

    public GameObject InstantiateCharacter(Vector3 position, Quaternion rotation)
    {
        GameObject retval = PhotonNetwork.Instantiate("PhotonCC", position, rotation);

        return retval;

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

        _playerSpawner.SpawnPlayer(NetworkManager.PlayerID);
        
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

    private void Update()
    {
    }

    public void GameEnd(bool victory)
    {
        gamePhase = IGameMaster.Phase.End;
    }

}