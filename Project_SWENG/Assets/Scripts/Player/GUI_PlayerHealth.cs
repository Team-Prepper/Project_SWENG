using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class GUI_PlayerHealth : MonoBehaviour
{
    [SerializeField] Health health;

    [SerializeField] Image healthFront;
    [SerializeField] Image healthBack;
    [SerializeField] Image afterimage;
    [SerializeField] TextMeshProUGUI hpText;

    int curHealth;
    int maxHealth;

    float duration = 0.1f;

    private void Awake()
    {
        Health.EventDamaged += GUI_Damaged;
    }

    void GUI_Damaged(object sender, IntEventArgs e)
    {
        SetHealth(e.Value);

        while (Mathf.Approximately(curHealth, e.Value))
        {
            float decrease = Mathf.Lerp(curHealth, e.Value, duration);
        }
    }

    void SetHealth(int newHealth)
    {
        healthFront.fillAmount = (float)(newHealth / health.maxHealth);
        healthBack.fillAmount  = (float)(newHealth / health.maxHealth);
    }
}
