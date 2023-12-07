using System;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UISystem;
using UnityEngine.UI;
using TMPro;
using Character;

public class GameManager : MonoSingletonPun<GameManager>
{
    public GameObject player;
    public List<GameObject> enemies;
    public List<GameObject> bossEnemies;
    public GameObject day;
    public GameObject night;

    private bool isGameOver = false;

    [SerializeField] private EnemySpawner spawner;
    [SerializeField] private bool[] _playerTurnEndArray;
    public Dictionary<string, int> playerDmgDashboard = new Dictionary<string, int>();
    
    public Button turnEndButton;

    public int remainLife = 5;
    public Transform respawnPos;
    [SerializeField] private bool StageBossSpawned = false;

    [Header("PlayerTotalHealth")]
    [SerializeField] private TextMeshProUGUI healthCount;

    [Header("GameEnd")] 
    [SerializeField] private GameObject victoryLevel;
    [SerializeField] private GameObject victoryCam;
    [SerializeField] private GameObject loseLevel;
    [SerializeField] private GameObject loseCam;
    [SerializeField] private GameObject dashboard;
    [SerializeField] private DashboardManager dashboardManager;

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
        dashboard.SetActive(false);
        victoryCam.SetActive(false);
        loseLevel.SetActive(false);
        HealthCountHandler();
        foreach(var player in PhotonNetwork.CurrentRoom.Players)
        {
            playerDmgDashboard.Add(player.Value.NickName,0);
        }
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
        if (isGameOver) return;
        gamePhase = Phase.SetDice;
        turnEndButton.interactable = true;
        changeDayNight();
        UIManager.Instance.UseDice(player.GetComponent<DicePoint>());
        player.GetComponent<PlayerController>().canUseSkill = true;
        NextPhase();
    }

    // Turn End Button Trigger
    public void PlayerTurnEnd()
    {
        photonView.RPC("PlayerTurnEndToServer", RpcTarget.MasterClient, NetworkManager.PlayerID);
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
        photonView.RPC("ServerTurnEnd", RpcTarget.All, bossEnemies.Count);
        
        
    }

    [PunRPC]
    private void ServerTurnEnd(int bossCnt)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            for (int i = 0; i < _playerTurnEndArray.Length; i++)
            {
                _playerTurnEndArray[i] = false;
            }
        }

        gamePhase = Phase.EnemyPhase;
        EnemyTurn();
        if(PhotonNetwork.IsMasterClient)
        {
            if (StageBossSpawned == false && bossCnt == 0)
            {
                Debug.Log("ALL BOSS IS DEAD");
                GameObject bossEnemy = spawner.SpawnBoss();
                GameManager.Instance.enemies.Add(bossEnemy);
                StageBossSpawned = true;
            }
        }

        if(isGameOver == false)
            Invoke("PlayerTurnStandBy", 3f);
    }
    
    public void EnemyTurn()
    {
        changeDayNight();
        foreach (GameObject enemy in enemies)
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
        for (int i = 0; i < PhotonNetwork.CountOfPlayersInRooms; i++)
        {
            _playerTurnEndArray[i] = false;
        }
    }

    public void HealthCountHandler()
    {
        photonView.RPC("HealthCount", RpcTarget.All, remainLife);
    }

    [PunRPC]
    private void HealthCount(int life)
    {
        healthCount.text = life.ToString();
    }



    public void CalTotalAttackDamageHandler(int damage)
    {
        photonView.RPC("CalAttackDamageToDashboardNick",RpcTarget.MasterClient, PhotonNetwork.NickName, damage);
    }
    
    [PunRPC]
    private void CalAttackDamageToDashboardNick(string playerNickName, int damage)
    {
        if(playerDmgDashboard.ContainsKey(playerNickName))
            playerDmgDashboard[playerNickName] += damage;
        else
            playerDmgDashboard.Add(playerNickName, damage);
    }

    public void GameEnd(bool victory)
    {
        photonView.RPC("NetworkGameEnd", RpcTarget.All, victory);
    }

    [PunRPC]
    private void NetworkGameEnd(bool victory)
    {
        isGameOver = true;
        if (victory)
        {
            victoryLevel.SetActive(true);
            victoryCam.SetActive(true);
        }
        else
        {
            loseLevel.SetActive(true);
            loseCam.SetActive(true);
        }
        dashboard.SetActive(true);
        dashboardManager.ShowDashboardHandler(victory);
        
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