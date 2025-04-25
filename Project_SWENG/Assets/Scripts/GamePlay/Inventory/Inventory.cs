using UnityEngine;
using System.Collections.Generic;
using System;
using EHTool;

public class Inventory : MonoBehaviour , IObservable<Inventory> {

    public IList<string> ItemList { get; private set; }
    [SerializeField] private int _budget = 5;

    private ISet<IObserver<Inventory>> _observers
        = new HashSet<IObserver<Inventory>>();

    private ICharacterController _cc;

    public IDisposable Subscribe(IObserver<Inventory> observer)
    {
        if (!_observers.Contains(observer))
        {
            _observers.Add(observer);

            observer.OnNext(this);
        }

        return new Unsubscriber<Inventory>(_observers, observer);
    }


    public bool IsFull() {
        return ItemList.Count >= _budget;
    }

    public void SetCC(ICharacterController cc) {
        _cc = cc;
        ItemList = new List<string>();
    }

    public void AddItem(string newItem) {
        if (IsFull()) return;
        ItemList.Add(newItem);
        Notify();
    }

    public void RemoveItem(int idx) {
        ItemList.RemoveAt(idx);
        Notify();
    }

    public void UseItem(int idx) {
        ItemManager.Instance.GetItemData(ItemList[idx]).GetItem().Action(_cc);
        RemoveItem(idx);
    }

    private void Notify() {
        foreach(var obs in _observers) {
            obs.OnNext(this);
        }
    }

}