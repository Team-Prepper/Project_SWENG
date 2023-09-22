using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : Character
{
    public int maxHealth;

    [SerializeField] int atkPoint = 3;

    public static event EventHandler<IntEventArgs> EventRecover;
    public static event EventHandler<IntEventArgs> EventDamaged;

    private Unit unit;

    private void Awake()
    {
        stat.curHP = maxHealth;
        unit = GetComponent<Unit>();
    }

    public int Recover(int val)
    {
        if (stat.curHP <= 0) return 0;

        stat.curHP += val;

        if (stat.curHP > maxHealth)
        {
            stat.curHP = maxHealth;
        }

        EventRecover?.Invoke(this, new IntEventArgs(stat.curHP));
        return stat.curHP;
    }

    public bool CanAttack() {
        return unit.dicePoints >= atkPoint;
    }

    public override void AttackAct() {
        unit.dicePoints -= atkPoint;
        EffectManager.Instance.SetTarget(gameObject);
        StartCoroutine(EffectManager.Instance.ShowImpactVfx(0));
    }

    public override void DamageAct()
    {
        base.DamageAct();
        EventDamaged?.Invoke(this, new IntEventArgs(stat.curHP));
    }

    public override void DieAct()
    {
        base.DieAct();
    }
}
