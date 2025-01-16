using UnityEngine;
using UnityEngine.UI;
using EHTool.UtilKit;

public class GUI_EnemyHealth : IHealthUI
{
    [SerializeField] Image healthBar;

    private void Start()
    {
        healthBar.fillAmount = 1;
    }

    public override void UpdateGUI(GaugeValue<int> value)
    {
        healthBar.fillAmount = value.ConvertToRate();
    }

}
