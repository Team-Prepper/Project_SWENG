using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    private Animator _animator;

    bool isDead;

    [SerializeField] int curHealth;
    public int maxHealth;

    public static event EventHandler<IntEventArgs> EventRecover;
    public static event EventHandler<IntEventArgs> EventDamaged;

    private void Awake()
    {
        _animator = GetComponent<Animator>();   
    }

    private void Start()
    {
        curHealth = maxHealth;
    }

    public int Recover(int val)
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

    public int Damaged(int val)
    {
        if (isDead) return 0;

        if (val > curHealth)
        {
            curHealth = 0;
            isDead = true;
            _animator.SetTrigger("Die");
            EventDamaged?.Invoke(this, new IntEventArgs(0));
            return 0;
        }

        curHealth -= val;
        _animator.SetTrigger("Hit");
        EventDamaged?.Invoke(this, new IntEventArgs(curHealth));
        return curHealth;
    }
}
