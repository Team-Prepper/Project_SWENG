using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DiceManager : MonoBehaviour
{
    public static DiceManager Instance { get; private set; }

    [SerializeField] private GameObject diceImage;
    [SerializeField] private Button diceBTN;
    [SerializeField] private Dice d20;
    
    private void Awake()
    {
        Instance = this;
    }

    private void OnEnable()
    {
        Unit.EventDicePoint += HandleDiceText;
        AttackManager.EventBaseAtk += HandleDiceText;
    }

    private void OnDisable()
    {
        Unit.EventDicePoint -= HandleDiceText;
        AttackManager.EventBaseAtk -= HandleDiceText;
    }
    
    public void RollingDice()
    {
        diceImage.SetActive(true);
        d20.Rolling();
        diceBTN.interactable = false;
    }
    
    private void HandleDiceText(object sender, IntEventArgs e)
    {
        StartCoroutine(HideCam());
    }

    IEnumerator HideCam()
    {
        yield return new WaitForSeconds(0.5f);
        diceImage.SetActive(false);
    }

    public void DiceStandBy()
    {
        diceImage.SetActive(true);
        diceBTN.interactable = true;
    }

}
