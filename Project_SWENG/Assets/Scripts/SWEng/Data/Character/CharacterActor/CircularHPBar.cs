using UnityEngine;
using UnityEngine.UI;
using System;
//using SWEng.GamePlay;

public class CircularHPBar : MonoBehaviour//, IObserver<IStatus>
{
    [SerializeField] private Image healthBar;

    public void OnCompleted()
    {

    }

    public void OnError(Exception error)
    {
        
    }

    public void OnNext()//IStatus value)
    {
        //healthBar.fillAmount = (float)value.CurHP / value.MaxHP;
    }

    private void Start()
    {
        healthBar.fillAmount = 1;
    }


}
