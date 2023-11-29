using LangSystem;
using ObserberPattern;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Xml.Serialization;
using UnityEngine;

public class InventoryManager : MonoSingleton<InventoryManager>, ISubject 
{
    private EquipManager _equipManager;

    private Item _helmet = null;
    private Item _armor  = null;
    public Item Weapon = null;
    private Item _shield = null;

    List<IObserver> _observers = new List<IObserver>();

    // EquipManger.Instance,EquipItem -> resetItem + setItem

    public void SetEquipManager(GameObject player)
    {
        _equipManager = player.GetComponent<EquipManager>();
    }

    public List<Item> GetItems() { 
        List<Item> retval = new List<Item>();
        if (_helmet) retval.Add(_helmet);
        if (_armor) retval.Add(_armor);
        if (Weapon) retval.Add(Weapon);
        if (_shield) retval.Add(_shield);

        return retval;
    }

    public void GetItem(Item item)
    {
        if(item.itemHex != null)
            item.itemHex.Item = null;

        switch (item.type)
        {
            case Item.ItemType.Helmet:
                _helmet = item;
                _equipManager.EquipHelmet(item);
                break;
            case Item.ItemType.Armor:
                _armor = item;
                _equipManager.EquipArmor(item);
                break;
            case Item.ItemType.Weapon:
                Weapon = item;
                _equipManager.EquipWeapon(item);
                break;
            case Item.ItemType.Shield:
                _shield = item;
                _equipManager.EquipShield(item);
                break;
        }

        NotifyToObserver();
    }

    public void AddObserver(IObserver ops)
    {
        _observers.Add(ops);
        ops.Notified();
    }

    public void RemoveObserver(IObserver ops)
    {
        _observers.Remove(ops);
    }

    public void NotifyToObserver()
    {
        List<IObserver> needRemove = new List<IObserver>();


        foreach (IObserver ops in _observers) {
            if (ops == null) {
                needRemove.Add(ops);
                continue;
            }
            ops.Notified();
        }
        foreach (IObserver ops in needRemove)
        {
            RemoveObserver(ops);
        }
    }
}
