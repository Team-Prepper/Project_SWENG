using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DiceManager : MonoBehaviour
{
    public static DiceManager Instance;

    [SerializeField] private GameObject dice;
    [SerializeField] private GameObject diceImage;
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] private Button diceBTN;
    private Dice d20;
    
    private void Awake()
    {
        Instance = this;
        d20 = dice.GetComponent<Dice>();
    }

    private void OnEnable()
    {
        Unit.EventDicePoint += HandleDiceText;
        AttackManager.EventBaseAtk += HandleDiceText;
        if (text != null)
            text.text = "";
    }

    private void OnDisable()
    {
        Unit.EventDicePoint -= HandleDiceText;
        AttackManager.EventBaseAtk -= HandleDiceText;
        if (text != null)
            text.text = "";
    }
    
    public void RollingDice()
    {
        diceImage.SetActive(true);
        text.text = "";
        d20.Rolling();
        diceBTN.interactable = false;
    }
    
    private void HandleDiceText(object sender, IntEventArgs e)
    {
        if (text != null)
        {
            if (e.Value == 0)
                text.text = "";
            else
                text.text = e.Value.ToString();
        }
           
        StartCoroutine(HideCam());
    }

    IEnumerator HideCam()
    {
        yield return new WaitForSeconds(0.5f);
        diceImage.SetActive(false);
    }

    public void DiceStandBy()
    {
        StartCoroutine(ShowCam());
    }

    IEnumerator ShowCam()
    {
        yield return new WaitForSeconds(0.1f);
        diceImage.SetActive(true);
        diceBTN.interactable = true;
    }
}
