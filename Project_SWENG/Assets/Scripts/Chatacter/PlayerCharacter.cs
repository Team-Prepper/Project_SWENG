using CharacterSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UISystem;
using UnityEngine;

public class PlayerCharacter : Character,
    IMoveable, IDicePoint {

    [SerializeField] int _usePointAtAttack = 3;
    [SerializeField] float _movementDuration = 1f;
    [SerializeField] GameObject playerLight;

    public bool canUseSkill = true;
    bool rollDice = false;

    [SerializeField] ParticleSystem LevelUpEffect;

    [SerializeField] int _dicePoint;

    public void SetHealthUI(IHealthUI ui)
    {
        _healthUI = ui;
        _healthUI.UpdateGUI(stat.HP);
    }

    public void Move(Queue<Vector3> path)
    {
        StartCoroutine(_RotationCoroutine(path, 0.1f));
    }

    public override IList<Action> GetCanDoAction()
    {
        List<Action> list = new List<Action>();

        if (rollDice == false) list.Add(Action.Dice);
        if (GetPoint() >= _usePointAtAttack) list.Add(Action.Attack);
        if (GetPoint() >= 2) list.Add(Action.Move);

        return list;
    }

    private IEnumerator _RotationCoroutine(Queue<Vector3> path, float rotationDuration)
    {
        Quaternion startRotation = transform.rotation;
        HexGrid.Instance.GetTileAt(HexCoordinate.ConvertFromVector3(transform.position)).Entity = null;

        if (anim)
            anim.SetBool("IsWalk", true);

        foreach (Vector3 targetPos in path)
        {
            Vector3 direction = targetPos - transform.position;
            Quaternion endRotation = Quaternion.LookRotation(direction, Vector3.up);

            float timeElapsed = 0;

            // È¸Àü

            transform.rotation = endRotation;

            if (Mathf.Approximately(Mathf.Abs(Quaternion.Dot(startRotation, endRotation)), 1.0f) == false)
            {

                while (timeElapsed < rotationDuration)
                {
                    timeElapsed += Time.deltaTime;
                    float lerpStep = timeElapsed / rotationDuration; // 0-1
                    transform.rotation = Quaternion.Lerp(startRotation, endRotation, lerpStep);
                    yield return null;
                }
                transform.rotation = endRotation;
            }

            Vector3 startPosition = transform.position;

            HexCoordinate newHexPos = HexCoordinate.ConvertFromVector3(targetPos);
            Hex goalHex = HexGrid.Instance.GetTileAt(newHexPos);
            goalHex.CloudActiveFalse();
            UsePoint(goalHex.Cost);

            timeElapsed = 0;

            while (timeElapsed < _movementDuration)
            {

                timeElapsed += Time.deltaTime;
                float lerpStep = timeElapsed / _movementDuration;
                transform.position = Vector3.Lerp(startPosition, targetPos, lerpStep);
                yield return null;
            }
            transform.position = targetPos;

        }
        CamMovement.Instance.IsPlayerMove = false;

        HexGrid.Instance.GetTileAt(HexCoordinate.ConvertFromVector3(transform.position)).Entity = gameObject;
        if (anim)
            anim.SetBool("IsWalk", false);

        _cc.ActionEnd();

    }

    public override int GetTeamIdx()
    {
        return 0;
    }

    public void UsePoint(int usingAmount)
    {
        if (_dicePoint < usingAmount)
        {
            return;
        }
        _dicePoint -= usingAmount;
    }

    public int GetPoint()
    {
        return _dicePoint;
    }

    public void SetPoint(int setValue)
    {
        _dicePoint = setValue;
        rollDice = true;
        _cc.ActionEnd();
    }

    public override void SetPlay()
    {
        base.SetPlay();
        rollDice = false;
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

    public override void AttackAct(bool isSkill)
    {
        base.AttackAct(isSkill);
        UsePoint(_usePointAtAttack);
        _cc.ActionEnd();
    }

    public override int GetAttackValue()
    {
        return stat.GetAttackValue() +
            (InventoryManager.Instance.Weapon ? InventoryManager.Instance.Weapon.value : 0);
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
        HexGrid.Instance.GetTileAt(transform.position).Entity = null;
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