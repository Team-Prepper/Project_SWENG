using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using CharacterSystem;

public class GUI_PlayerHealth : MonoBehaviour, IHealthUI
{

    [SerializeField] Image _healthFront;
    [SerializeField] Image _healthBack;
    [SerializeField] Image _afterimage;
    [SerializeField] TextMeshProUGUI _hpText;
    
    public float lerpSpeed = 100f;

    float _curValue;

    public void UpdateGUI(GaugeValue<int> value)
    {
        _healthFront.fillAmount = value.ConvertToRate();
        _healthBack.fillAmount = value.ConvertToRate();
        _hpText.text = value.Value.ToString() + " / " + value.MaxValue;

        StartCoroutine(LerpValue(value.Value));

    }
    
    private IEnumerator LerpValue(float endValue)
    {
        float elapsedTime = 0f;
        float startValue = _curValue;

        _afterimage.fillAmount = endValue;

        while (elapsedTime < 1f)
        {
            _curValue = Mathf.Lerp(startValue, endValue, elapsedTime);
            elapsedTime += Time.deltaTime * lerpSpeed;
            yield return null;
        }
    }

}
