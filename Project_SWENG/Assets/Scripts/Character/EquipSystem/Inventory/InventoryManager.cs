using EHTool;
using System;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoSingleton<InventoryManager>, IObservable<InventoryManager>
{
    private EquipManager _equipManager;

    private Item _helmet = null;
    private Item _armor  = null;
    public Item Weapon = null;
    private Item _shield = null;

    ISet<IObserver<InventoryManager>> _observers = new HashSet<IObserver<InventoryManager>>();

    // EquipManger.Instance,EquipItem -> resetItem + setItem

    public IDisposable Subscribe(IObserver<InventoryManager> observer)
    {
        if (!_observers.Contains(observer))
        {
            _observers.Add(observer);

            observer.OnNext(this);
        }

        return new Unsubscriber<InventoryManager>(_observers, observer);
    }

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

    public void NotifyToObserver()
    {
        foreach (IObserver<InventoryManager> target in _observers)
        {
            target.OnNext(this);
        }
    }
}
