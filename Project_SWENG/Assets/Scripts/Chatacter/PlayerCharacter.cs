using CharacterSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UISystem;
using UnityEngine;

public class PlayerCharacter : Character,
    IMoveable, IDicePoint {

    [SerializeField] int _usePointAtAttack = 3;
    [SerializeField] GameObject playerLight;

    public bool canUseSkill = true;
    bool _rollDice = false;

    [SerializeField] ParticleSystem LevelUpEffect;

    public override void SetPoint(int setValue)
    {
        _rollDice = true;
        base.SetPoint(setValue);
    }

    public void SetHealthUI(IHealthUI ui)
    {
        _healthUI = ui;
        _healthUI.UpdateGUI(stat.HP);
    }

    public override IList<Action> GetCanDoAction()
    {
        List<Action> list = new List<Action>();

        if (_rollDice == false) list.Add(Action.Dice);
        if (GetPoint() >= _usePointAtAttack) list.Add(Action.Attack);
        if (GetPoint() >= 2) list.Add(Action.Move);

        return list;
    }

    public override int GetTeamIdx()
    {
        return 0;
    }

    public override void SetPlay()
    {
        _rollDice = false;
        _cc.ActionEnd();
    }

    public override void Initial(ICharacterController cc)
    {
        base.Initial(cc);
        stat.HP.FillMax();
        //playerLight.SetActive(false);
    }

    public int Recover(int val)
    {
        if (!stat.IsAlive()) return 0;

        stat.Recover(val);

        //EventChangeHp?.Invoke(this, new IntEventArgs(stat.HP.Value));
        return stat.HP.Value;
    }

    public override int GetAttackValue()
    {
        return stat.GetAttackValue() +
            (InventoryManager.Instance.Weapon ? InventoryManager.Instance.Weapon.value : 0);
    }

    public override void DoAttact()
    {
        IAttack attack = new BasicTargetingAttack(_cc, this, transform.position, 10, GetPoint());
    }

    public void AttackVfx()
    {
        EffectManager.Instance.SetTarget(gameObject);
        if (InventoryManager.Instance.Weapon)
        {
            int weaponID = InventoryManager.Instance.Weapon.id;
            EffectManager.Instance.ShowImpactVfxHandler(weaponID);
        }
    }

    public override void DieAct()
    {
        //HexGrid.Instance.GetTileAt(transform.position).Entity = null;
    }

    public void EquipItemHandler(Item item)
    {
        // itemType 0 : helmet
        // itemType 1 : Armor
        // itemType 2 : shield

        switch (item.type)
        {
            case Item.ItemType.Helmet:
                EquipItem(0, item.value);
                break;
            case Item.ItemType.Armor:
                EquipItem(1, item.value);
                break;
            case Item.ItemType.Shield:
                EquipItem(2, item.value);
                break;
        }

        //EventEquip?.Invoke(this, null);
    }

    public void EquipItem(int itemType, int value)
    {
        if (!stat.IsAlive()) return;

        switch (itemType)
        {
            case 0:
                stat.HP.AddMaxValue(value);
                stat.SetDef(value, true);
                stat.SetAttackPower(value, true);
                break;
            case 1:
                stat.HP.AddMaxValue(value);
                break;
            case 2:
                stat.SetDef(value, true);
                break;
        }
    }

    public void UnEquipItemHandler(Item item)
    {
        // itemType 0 : helmet
        // itemType 1 : Armor
        // itemType 2 : shield

        switch (item.type)
        {
            case Item.ItemType.Helmet:
                UnequipItem(0, item.value);
                break;
            case Item.ItemType.Armor:
                UnequipItem(1, item.value);
                break;
            case Item.ItemType.Shield:
                stat.SetDef(item.value, false);
                break;
        }
        //EventEquip?.Invoke(this, null);
    }

    public void UnequipItem(int itemType, int value)
    {
        if (!stat.IsAlive()) return;

        switch (itemType)
        {
            case 0:
                stat.HP.SubMaxValue(value);
                stat.SetDef(value, false);
                stat.SetAttackPower(value, false);
                break;
            case 1:
                stat.HP.SubMaxValue(value);
                break;
            case 2:
                stat.SetDef(value, false);
                break;
        }
    }

    public void GetExp(int val)
    {
        if (stat.GetExp(val))
        {
            //LevelUpEffect.Play();
            //EventEquip?.Invoke(this, null);
        }

    }
}