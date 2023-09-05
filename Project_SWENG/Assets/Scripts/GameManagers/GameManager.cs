using System;
using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Singleton")]
    static GameManager instance;

    public GameObject player;

    public enum Phase
    {
        Ready,
        Start,
        SetDice,
        DiceRolling,
        Movement,
        Interaction
    }
    public Phase gamePhase;

    public static GameManager Instance
    {
        get
        {
            Init();
            return instance;
        }
    }

    static void Init()
    {
        if (instance == null)
        {
            GameObject obj = GameObject.Find("GM");
            if (obj == null)
            {
                obj = new GameObject { name = "GM" };
                obj.AddComponent<GameManager>();
            }
            DontDestroyOnLoad(obj);
            instance = obj.GetComponent<GameManager>();
        }
    }

    void Awake()
    {
        gamePhase = Phase.Ready;
    }

    private void Update()
    {
        if(gamePhase == Phase.Start)
        {
            NextPhase();
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
        //do something EndPhase
        PlayerTurnStandBy();
    }
}
