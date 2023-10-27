using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using Character;

public class GUI_PlayerHealth : MonoBehaviour
{
    PlayerController _manger;

    [SerializeField] Image healthFront;
    [SerializeField] Image healthBack;
    [SerializeField] Image afterimage;
    [SerializeField] TextMeshProUGUI hpText;
    
    public float lerpSpeed = 100f;

    float curValue;

    private void Awake()
    {
        PlayerController.EventDamaged += GUI_Damaged;
    }

    public void SetPlayerHealth(GameObject player)
    {
        _manger = player.GetComponent<PlayerController>();
        SetHealth(_manger.stat.GetHP().Value);
        afterimage.fillAmount = 1;
        curValue = _manger.stat.GetHP().Value;
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
            afterimage.fillAmount = _manger.stat.GetHP().ConvertToRate();
            elapsedTime += Time.deltaTime * lerpSpeed;
            yield return null;
        }
    }

    void SetHealth(int newHealth)
    {
        healthFront.fillAmount = _manger.stat.GetHP().ConvertToRate();
        healthBack.fillAmount  = _manger.stat.GetHP().ConvertToRate();
        hpText.text = newHealth.ToString() + " / " + _manger.stat.GetHP().MaxValue;
    }
}
