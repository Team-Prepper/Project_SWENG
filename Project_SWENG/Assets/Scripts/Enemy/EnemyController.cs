using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public EnemyStat enemyStat;
    public Animator ani;
    [SerializeField] GUI_EnemyHealth healthGUI;

    public Hex curHex;

    private void Awake()
    {
        if(enemyStat == null)
            enemyStat = GetComponent<EnemyStat>();
        
        if(ani == null)
            ani = GetComponent<Animator>();

        if(healthGUI == null)
            healthGUI = GetComponentInChildren<GUI_EnemyHealth>();
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
            EnemyDeadHandler();
            return;
        }
        ani.SetTrigger("Hit");
        healthGUI.UpdateGUI((float)enemyStat.curHp / enemyStat.maxHp);
    }

    private void EnemyDeadHandler()
    {
        Hex curHex = HexGrid.Instance.GetHexFromPosition(this.gameObject.transform.position);
        curHex.Entity = null;
        EnemySpawner.Instance.enemyList.Remove(this.gameObject);
        Destroy(this.gameObject, 1f);
    }
}
