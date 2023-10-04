using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class GameState : NetworkBehaviour
{
    public enum EGameState
    {
        Off,
        Lobby,
        MapCreating,
        TurnPlayer,
        TurnEnemy
    }
    
    protected StateMachine<EGameState> StateMachine = new StateMachine<EGameState>();

    public override void Spawned()
    {
        StateMachine[EGameState.Off].onExit = newState =>
        {
            Debug.Log($"Exited {EGameState.Off} to {newState}");

            if (Runner.IsServer)
            {
            }

            if (Runner.IsPlayer) // [PLAYER] Off -> *
            {
                GUI_Lobby.Instance.InitPregame(Runner);
            }
        };
    }
}
