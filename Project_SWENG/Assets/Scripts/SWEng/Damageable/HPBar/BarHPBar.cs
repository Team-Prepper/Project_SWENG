using System.Collections;
using System;
using UnityEngine;
using UnityEngine.UI;

public class BarHPBar : HPBarBase
{

    [SerializeField] private Image _healthFront;
    [SerializeField] private Image _healthBack;
    [SerializeField] private Image _afterimage;
    [SerializeField] private Text _hpText;
    
    public float lerpSpeed = 100f;

    private float _curValue;

    protected override void SetRatio(float ratio, int cur, int max)
    {
        _healthFront.fillAmount = ratio;
        _healthBack.fillAmount = ratio;
        _hpText.text = string.Format("{0} / {1}", cur, max);
            
        StartCoroutine(LerpValue(ratio));
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
