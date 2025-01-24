using EHTool;
using System;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoSingleton<InventoryManager>, IObservable<InventoryManager>
{
    private EquipManager _equipManager;

    private ItemData _helmet = null;
    private ItemData _armor  = null;
    public ItemData Weapon = null;
    private ItemData _shield = null;

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

    public List<ItemData> GetItems() { 
        List<ItemData> retval = new List<ItemData>();
        if (_helmet) retval.Add(_helmet);
        if (_armor) retval.Add(_armor);
        if (Weapon) retval.Add(Weapon);
        if (_shield) retval.Add(_shield);

        return retval;
    }

    public void GetItem(ItemData item)
    {
        if (item.itemHex != null)
            //item.itemHex.SetItem(null);

        switch (item.type)
        {
            case ItemData.ItemType.Helmet:
                _helmet = item;
                _equipManager.EquipHelmet(item);
                break;
            case ItemData.ItemType.Armor:
                _armor = item;
                _equipManager.EquipArmor(item);
                break;
            case ItemData.ItemType.Weapon:
                Weapon = item;
                _equipManager.EquipWeapon(item);
                break;
            case ItemData.ItemType.Shield:
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
