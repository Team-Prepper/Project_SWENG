using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] EnemyStat enemyStat;
    [SerializeField] Animator ani;
    [SerializeField] EnemyHealthGUI healthGUI;

    private void OnEnable()
    {
        if(enemyStat == null)
            enemyStat = GetComponent<EnemyStat>();
        
        if(ani == null)
            ani = GetComponent<Animator>();

        if(healthGUI == null)
            healthGUI = GetComponentInChildren<EnemyHealthGUI>();
    }

    public void DamagedHandler(int damage)
    {
        if (enemyStat.isDie) return;

        Debug.Log("Enemy Take Damage : " + damage);

        enemyStat.curHp -= damage;

        

        if (enemyStat.curHp <= 0)
        {
            enemyStat.curHp = 0;
            enemyStat.isDie = true;
            ani.SetTrigger("Die");
            healthGUI.UpdateGUI(0);
            return;
        }
        ani.SetTrigger("Hit");
        healthGUI.UpdateGUI(enemyStat.curHp/ enemyStat.maxHp);
    }
}
