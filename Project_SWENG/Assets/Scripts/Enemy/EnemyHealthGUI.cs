using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class EnemyHealthGUI : MonoBehaviour
{
    [SerializeField] Image healthBar;

    private void OnEable()
    {
        healthBar.fillAmount = 1;
    }
    public void UpdateGUI(float value)
    {
        healthBar.fillAmount = value;
    }
}
