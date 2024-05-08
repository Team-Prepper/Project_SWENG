using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GUI_EnemyHealth : MonoBehaviour, IHealthUI
{
    [SerializeField] Image healthBar;

    private void Start()
    {
        healthBar.fillAmount = 1;
    }

    public void UpdateGUI(GaugeValue<int> value)
    {
        healthBar.fillAmount = value.ConvertToRate();
    }
}
