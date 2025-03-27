using EHTool.UIKit;
using UnityEngine;
using System;

public class GUIInventory : GUIPopUp, IObserver<Inventory> {

    [SerializeField] private GUIUnitInventoryUnit[] _units;

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
        for (int i = 0; i < _units.Length; i++) {
            _units[i].SetItemInfor(value, i);
        }
    }

    public void Set(ICharacterController cc) {
        _cc = cc;
        _disposable = _cc.Inventory.Subscribe(this);
    }

    public override void Close() {
        _cc.ActionEnd();
        _disposable?.Dispose();
        base.Close();
    }

}