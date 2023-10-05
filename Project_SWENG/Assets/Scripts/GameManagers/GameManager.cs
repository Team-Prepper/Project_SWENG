using System;
using System.Collections;
using System.Collections.Generic;
using Fusion;
using Unity.AI.Navigation;
using UnityEngine;
using static GameState;

public class GameManager : NetworkBehaviour, INetworkRunnerCallbacks
{
    public static GameManager Instance;
    public static GameState State { get; private set; }
    
    public GameObject player;
    public List<GameObject> enemys;
    public GameObject day;
    public GameObject night;

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

    private void Awake()
    {
        
        if (Instance == null)
        {
            Instance = this;
            
            State = GetComponent<GameState>();
        }
        else
        {
            Destroy(gameObject);
        }
        gamePhase = Phase.Ready;
    }

    private void Update()
    {
        if(gamePhase == Phase.Start)
        {
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
        UIManager.Instance.UseDice(player.GetComponent<Unit>());
        NextPhase();
    }

    // Turn End Button Trigger
    public void PlayerTurnEnd()
    {
        gamePhase = Phase.EnemyPhase;
        EnemyTurn();
        Invoke("PlayerTurnStandBy",3f);
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
        if(gamePhase != Phase.EnemyPhase)
        {
            day.SetActive(true);
            night.SetActive(false);
        }
        else
        {
            day.SetActive(false );
            night.SetActive(true);
        }
    }

    // NETWORK
    
    public override void Spawned()
    {
        base.Spawned();
        if (Runner.IsServer)
        {
            State.Server_SetState(EGameState.Lobby);
        }

        Runner.AddCallbacks(this);
    }

    void INetworkRunnerCallbacks.OnConnectedToServer(NetworkRunner runner)
    {
        UIScreen.CloseAll();
    }
    
    public static void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
		Application.Quit();
#endif
    }
    
    void INetworkRunnerCallbacks.OnConnectFailed(NetworkRunner runner, Fusion.Sockets.NetAddress remoteAddress, Fusion.Sockets.NetConnectFailedReason reason) { }
    void INetworkRunnerCallbacks.OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason) { }
    void INetworkRunnerCallbacks.OnDisconnectedFromServer(NetworkRunner runner) { }
    void INetworkRunnerCallbacks.OnPlayerJoined(NetworkRunner runner, PlayerRef player) { }
    void INetworkRunnerCallbacks.OnPlayerLeft(NetworkRunner runner, PlayerRef player) { }
    void INetworkRunnerCallbacks.OnInput(NetworkRunner runner, NetworkInput input) { }
    void INetworkRunnerCallbacks.OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input) { }
    void INetworkRunnerCallbacks.OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token) { }
    void INetworkRunnerCallbacks.OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message) { }
    void INetworkRunnerCallbacks.OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList) { }
    void INetworkRunnerCallbacks.OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data) { }
    void INetworkRunnerCallbacks.OnReliableDataReceived(NetworkRunner runner, PlayerRef player, System.ArraySegment<byte> data) { }
    void INetworkRunnerCallbacks.OnSceneLoadDone(NetworkRunner runner) { }
    void INetworkRunnerCallbacks.OnSceneLoadStart(NetworkRunner runner) { }
    void INetworkRunnerCallbacks.OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken) { }
}
