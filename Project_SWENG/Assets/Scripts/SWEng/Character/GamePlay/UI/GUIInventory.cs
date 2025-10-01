using UnityEngine;
using System;
using SWEng;
using EasyH.Gaming.Inventory;
using EasyH.Unity.UI;

public class GUIInventory : GUIPopUp, IObserver<Inventory>
{

    [SerializeField] private GUIUnitInventoryUnit[] _units;
    [SerializeField] private GUIUnitItemDataBase _selectedItemInfor;

    private int _idx;

    private ICharacter _cc;

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
        _idx = Mathf.Min(_idx, _cc.Inventory.ItemList.Count - 1);

        for (int i = 0; i < _units.Length; i++)
        {
            _units[i].SetItemInfor(_cc.Inventory.ItemList, i, Select);
        }

        if (_idx < 0)
        {
            _selectedItemInfor.gameObject.SetActive(false);
            return;
        }

        Debug.Log(_cc.Inventory.ItemList[_idx]);
    }

    public void Select(int idx)
    {
        if (_idx >= 0)
        {
            _units[_idx].DisSelect();
        }
        _idx = idx;
        _selectedItemInfor.gameObject.SetActive(true);
        _selectedItemInfor.SetItemInfor(_cc.Inventory.ItemList[_idx]);
    }

    public void Set(ICharacter cc)
    {
        _cc = cc;
        _idx = -1;
        _disposable = _cc.Inventory.Subscribe(this);
    }

    public void Use()
    {
        if (_idx < 0) return;

        int idx = _idx;
        _idx = -1;

        ItemManager.Instance.
            GetItem(_cc.Inventory.ItemList[idx]).Action(_cc);

        _cc.Inventory.RemoveItem(idx);

    }

    public void Discard()
    {
        if (_idx < 0) return;
        int idx = _idx;
        _idx = -1;
        _cc.Inventory.RemoveItem(idx);

    }

    public override void Close()
    {
        _cc.ActionEnd();
        _disposable?.Dispose();
        base.Close();
    }

}