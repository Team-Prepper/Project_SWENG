using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] EnemyStat enemyStat;
    [SerializeField] Animator ani;

    private void OnEnable()
    {
        if(enemyStat == null)
            enemyStat = GetComponent<EnemyStat>();
        
        if(ani == null)
            ani = GetComponent<Animator>();
    }

    public void DamagedHandler(int damage)
    {
        Debug.Log("Enemy Take Damage : " + damage);

        enemyStat.curHp -= damage;
        if(enemyStat.curHp < 0)
        {
            enemyStat.curHp = 0;
            enemyStat.isDie = true;
            ani.SetTrigger("Die");
            return;
        }
        ani.SetTrigger("Hit");
    }
}
