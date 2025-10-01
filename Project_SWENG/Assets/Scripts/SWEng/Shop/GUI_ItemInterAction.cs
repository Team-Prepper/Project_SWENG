using UnityEngine;
using System;
using EasyH.Unity.UI;

public class GUI_ItemInterAction : GUIPopUp
{
    [SerializeField] private GUIUnitItemDataIcon _infor;

    private Action _interactionEvent;
    private Action _closeEvent;

    public void SetItem(string item) {
        
        _infor.SetItemInfor(item);
        
    }

    public void InteractionEventSet(Action action) {
        _interactionEvent += action;
    }

    public void CloseEvent(Action action) {
        _closeEvent = action;
    }

    public void InterAction() {
        _interactionEvent?.Invoke();
    }

    public override void Close()
    {
        _closeEvent?.Invoke();
        base.Close();
    }

}