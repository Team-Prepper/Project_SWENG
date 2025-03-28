using EHTool.UIKit;
using UnityEngine;
using System;

public class GUIInventory : GUIPopUp, IObserver<Inventory> {

    [SerializeField] private GUIUnitInventoryUnit[] _units;
    [SerializeField] private IGUIUnitItemInfor _selectedItemInfor;

    private int _idx; 

    private ICharacterController _cc;

#nullable enable
    private IDisposable? _disposable;

    public void OnCompleted()
    {

    }

    public void OnError(Exception error)
    {

    }

    public void OnNext(Inventory value)
    {
        _idx = Mathf.Min(_idx, _cc.Inventory.ItemList.Count);

        for (int i = 0; i < _units.Length; i++) {
            _units[i].SetItemInfor(_cc.Inventory.ItemList, i, Select);
        }

        if (_idx < 0) {
            _selectedItemInfor.gameObject.SetActive(false);
            return;
        }

        Debug.Log(_cc.Inventory.ItemList[_idx]);
    }

    public void Select(int idx)
    {
        if (_idx >= 0) {
            _units[_idx].DisSelect();
        }
        _idx = idx;
        _selectedItemInfor.gameObject.SetActive(true);
        _selectedItemInfor.SetItemInfor(_cc.Inventory.ItemList[_idx]);
    }

    public void Set(ICharacterController cc) {
        _cc = cc;
        _idx = -1;
        _disposable = _cc.Inventory.Subscribe(this);
    }

    public void Use() {
        if (_idx < 0) return;
        _cc.Inventory.UseItem(_idx);
        _idx = -1;
    }

    public void Discard() {
        if (_idx < 0) return;
        _cc.Inventory.RemoveItem(_idx);
        _idx = -1;

    }

    public override void Close() {
        _cc.ActionEnd();
        _disposable?.Dispose();
        base.Close();
    }

}