using UnityEngine;
using UnityEngine.UI;
using System;

public class CircularHPBar : MonoBehaviour, IObserver<IStatus>
{
    [SerializeField] Image healthBar;

    public void OnCompleted()
    {

    }

    public void OnError(Exception error)
    {

    }

    public void OnNext(IStatus value)
    {
        healthBar.fillAmount = value.HP.ConvertToRate();
    }

    private void Start()
    {
        healthBar.fillAmount = 1;
    }


}
