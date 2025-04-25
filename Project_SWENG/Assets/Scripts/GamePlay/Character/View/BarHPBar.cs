using System.Collections;
using System;
using UnityEngine;
using UnityEngine.UI;

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
        _healthFront.fillAmount = value.HP.ConvertToRate();
        _healthBack.fillAmount = value.HP.ConvertToRate();
        _hpText.text = value.HP.Value.ToString() + " / " + value.HP.MaxValue;
        StartCoroutine(LerpValue(value.HP.ConvertToRate()));
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
