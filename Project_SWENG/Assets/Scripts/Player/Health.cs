using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    bool isDead;

    public int curHealth;
    public int maxHealth;

    public static event EventHandler<IntEventArgs> EventRecover;
    public static event EventHandler<IntEventArgs> EventDamaged;
    public static event EventHandler EventDead;

    int Recover(int val)
    {
        if (isDead) return 0;

        if (curHealth + val > maxHealth)
        {
            curHealth = maxHealth;
        }
        else
        {
            curHealth += val;
        }
        EventRecover?.Invoke(this, new IntEventArgs(curHealth));
        return curHealth;
    }

    int Damaged(int val)
    {
        if (isDead) return 0;

        if (val > curHealth)
        {
            curHealth = 0;
            isDead = true;
            EventDead?.Invoke(this, EventArgs.Empty);
            return 0;
        }

        curHealth -= val;
        EventDamaged?.Invoke(this, new IntEventArgs(curHealth));
        return curHealth;
    }
}
