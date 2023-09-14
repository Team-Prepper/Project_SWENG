using System;
using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    [Header("Singleton")]

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
}
