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
    
    [Networked] public EGameState Previous { get; set; }
    [Networked] public EGameState Current { get; set; }

    [Networked] TickTimer Delay { get; set; }
    [Networked] EGameState DelayedState { get; set; }
    
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
    
    
    //================================================================================//
    public override void FixedUpdateNetwork()
    {
        if (Runner.IsServer)
        {
            if (Delay.Expired(Runner))
            {
                Delay = TickTimer.None;
                Server_SetState(DelayedState);
            }
        }

        if (Runner.IsForward)
            StateMachine.Update(Current, Previous);
    }
    
    public void Server_SetState(EGameState st)
    {
        if (Current == st) return;
        Previous = Current;
        Current = st;
    }
	
    public void Server_DelaySetState(EGameState newState, float delay)
    {
        Debug.Log($"Delay state change to {newState} for {delay}s");
        Delay = TickTimer.CreateFromSeconds(Runner, delay);
        DelayedState = newState;
    }
}
