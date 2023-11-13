using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UISystem;
using UnityEngine.UI;

public class GameManager : MonoSingletonPun<GameManager>
{
    public GameObject player;
    public List<GameObject> enemys;
    public GameObject day;
    public GameObject night;

    [SerializeField] private bool[] _playerTurnEndArray;

    public Button turnEndButton;


    public enum Phase
    {
        Ready,
        Start,
        SetDice,
        DiceRolling,
        ActionPhase,
        AttackPhase,
        EnemyPhase,
    }
    public Phase gamePhase;

    private void Start()
    {
        _playerTurnEndArray = new bool[PhotonNetwork.CurrentRoom.PlayerCount];
        ResetPlayerTurn();
    }

    private void Update()
    {
        if (gamePhase == Phase.Start)
        {
            HexGrid.Instance.GetTileAt(player.transform.position).CloudActiveFalse();
            NextPhase();
            PlayerTurnStandBy();
        }
    }

    public void NextPhase()
    {
        int currentPhaseIndex = (int)gamePhase;
        int nextPhaseIndex = (currentPhaseIndex + 1) % Enum.GetValues(typeof(Phase)).Length;
        gamePhase = (Phase)nextPhaseIndex;
    }

    public void PlayerTurnStandBy()
    {
        gamePhase = Phase.SetDice;
        changeDayNight();
        UIManager.Instance.UseDice(player.GetComponent<DicePoint>());
        NextPhase();
    }

    // Turn End Button Trigger
    public void PlayerTurnEnd()
    {
        photonView.RPC("PlayerTurnEndToServer",RpcTarget.MasterClient,NetworkManager.PlayerID);
        turnEndButton.interactable = false;
    }

    [PunRPC]
    private void PlayerTurnEndToServer(int index)
    {
        _playerTurnEndArray[index] = true;
        CheckAllPlayerTurnEnd();
    }
    
    private void CheckAllPlayerTurnEnd()
    {
        foreach (var value in _playerTurnEndArray)
        {
            if (value == false) return;
        }
        photonView.RPC("ServerTurnEnd",RpcTarget.All, null);
        
        for (int i = 0; i < PhotonNetwork.CountOfPlayers; i++)
        {
            _playerTurnEndArray[i] = false;
        }
    }

    [PunRPC]
    private void ServerTurnEnd()
    {
        gamePhase = Phase.EnemyPhase;
        EnemyTurn();
        Invoke("PlayerTurnStandBy", 3f);
    }
    
    public void EnemyTurn()
    {
        changeDayNight();
        foreach (GameObject enemy in enemys)
        {
            EnemyAttack enemyAttack = enemy.GetComponent<EnemyAttack>();
            if (enemyAttack != null)
            {
                enemyAttack.EnemyAttackHandler();
            }
        }
    }

    void changeDayNight()
    {
        if (gamePhase != Phase.EnemyPhase)
        {
            day.SetActive(true);
            night.SetActive(false);
        }
        else
        {
            day.SetActive(false);
            night.SetActive(true);
        }
    }

    private void ResetPlayerTurn()
    {
        turnEndButton.interactable = true;
        for (int i = 0; i < PhotonNetwork.CountOfPlayers; i++)
        {
            _playerTurnEndArray[i] = false;
        }
    }

    

    // NETWORK

    public static void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
		Application.Quit();
#endif
    }
}