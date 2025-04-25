using UnityEngine;
using UnityEngine.UI;
using EHTool.LangKit;
using EHTool.UIKit;
using System;

public class GUI_ItemInterAction : GUIPopUp
{
    [SerializeField] private Image _itemIcon;
    [SerializeField] private Text _itemNameLabel;
    [SerializeField] private Text _itemInforLabel;

    private Action _interactionEvent;
    private Action _closeEvent;

    public void SetItem(string item) {

        ItemData itemData = ItemManager.Instance.GetItemData(item);

        _itemIcon.sprite = itemData.Icon;

        _itemNameLabel.text = LangManager.Instance.GetStringByKey(itemData.ItemName);
        _itemInforLabel.text = LangManager.Instance.GetStringByKey(itemData.ItemDesc);

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
