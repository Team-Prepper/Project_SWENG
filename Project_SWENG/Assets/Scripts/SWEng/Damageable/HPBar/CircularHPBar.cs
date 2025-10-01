using UnityEngine;
using UnityEngine.UI;

public class CircularHPBar : HPBarBase
{
    [SerializeField] private Image healthBar;

    protected override void SetRatio(float ratio, int cur, int max)
    {
        healthBar.fillAmount = ratio;
    }

    private void Start()
    {
        healthBar.fillAmount = 1;
    }


}
