using UnityEngine;
using UnityEngine.UI;
using EHTool.UtilKit;

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
