using System;
using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    [Header("Singleton")]

    public GameObject player;

    public enum Phase
    {
        Ready,
        Start,
        SetDice,
        DiceRolling,
        ActionPhase,
        EnemyPhase,
    }
    public Phase gamePhase;

    protected override void OnCreate()
    {
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
        DiceManager.Instance.DiceStandBy();
        NextPhase();
    }

    // Turn End Button Trigger
    public void PlayerTurnEnd()
    {
        gamePhase = Phase.EnemyPhase;
        //do something EndPhase
        PlayerTurnStandBy();
    }
}
