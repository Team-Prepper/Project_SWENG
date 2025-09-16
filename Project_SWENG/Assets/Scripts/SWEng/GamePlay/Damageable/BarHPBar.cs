using System.Collections;
using System;
using UnityEngine;
using UnityEngine.UI;
using SWEng.GamePlay;

public class BarHPBar : MonoBehaviour, IObserver<IStatus>
{

    [SerializeField] private Image _healthFront;
    [SerializeField] private Image _healthBack;
    [SerializeField] private Image _afterimage;
    [SerializeField] private Text _hpText;
    
    public float lerpSpeed = 100f;

    private float _curValue;

    public void OnCompleted()
    {

    }

    public void OnError(Exception error)
    {

    }

    public void OnNext(IStatus value)
    {
        float ratio = (float)value.CurHP / value.MaxHP;

        _healthFront.fillAmount = ratio;
        _healthBack.fillAmount = ratio;
        _hpText.text = string.Format("{0} / {1}",
            value.CurHP, value.MaxHP);
            
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
