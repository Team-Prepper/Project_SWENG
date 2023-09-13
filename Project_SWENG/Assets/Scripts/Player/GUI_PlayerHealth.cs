using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class GUI_PlayerHealth : MonoBehaviour
{
    PlayerHealth health;

    [SerializeField] Image healthFront;
    [SerializeField] Image healthBack;
    [SerializeField] Image afterimage;
    [SerializeField] TextMeshProUGUI hpText;
    
    public float lerpSpeed = 0.5f;
    public float updateInterval = 0.1f;

    float curValue;

    private void Awake()
    {
        PlayerHealth.EventDamaged += GUI_Damaged;
    }

    public void SetPlayerHealth(GameObject player)
    {
        health = player.GetComponent<PlayerHealth>();
        SetHealth(health.maxHealth);
        afterimage.fillAmount = 1;
        curValue = health.maxHealth;
    }

    void GUI_Damaged(object sender, IntEventArgs e)
    {
        SetHealth(e.Value);
        StartCoroutine(LerpValue(e.Value));
    }

    private IEnumerator LerpValue(float endValue)
    {
        float elapsedTime = 0f;
        float startValue = curValue;
        while (elapsedTime < 1f)
        {
            curValue = Mathf.Lerp(startValue, endValue, elapsedTime);
            afterimage.fillAmount = (float)(curValue / health.maxHealth);
            elapsedTime += Time.deltaTime * lerpSpeed;
            yield return new WaitForSeconds(updateInterval);
        }
    }

    void SetHealth(int newHealth)
    {
        healthFront.fillAmount = ((float)newHealth / health.maxHealth);
        healthBack.fillAmount  = ((float)newHealth / health.maxHealth);
        hpText.text = newHealth.ToString() + " / " + health.maxHealth.ToString();
    }
}
