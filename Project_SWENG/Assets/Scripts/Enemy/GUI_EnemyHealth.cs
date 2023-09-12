using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GUI_EnemyHealth : MonoBehaviour
{
    [SerializeField] Image healthBar;

    private void Start()
    {
        healthBar.fillAmount = 1;
    }

    public void UpdateGUI(float value)
    {
        healthBar.fillAmount = value;
    }
}
