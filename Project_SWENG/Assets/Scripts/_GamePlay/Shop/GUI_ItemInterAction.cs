using UnityEngine;
using UnityEngine.UI;
using TMPro;
using EHTool.LangKit;
using EHTool.UIKit;
using System;

public class GUI_ItemInterAction : GUIPopUp
{
    [SerializeField] private Image _itemIcon;
    [SerializeField] private Image _itemSkillIcon;
    [SerializeField] private Text _itemNameLabel;
    [SerializeField] private Text _itemInforLabel;
    [SerializeField] private Text _itemValueLabel;
    [SerializeField] private Text _itemSkillInfoLabel;

    Action _interactionEvent;
    Action _closeEvent;

    public void SetItem(string item) {

        ItemData itemData = ItemManager.Instance.GetItemData(item);

        _itemIcon.sprite = itemData.icon;
        _itemSkillIcon.sprite = itemData.skillIcon;

        _itemNameLabel.text = LangManager.Instance.GetStringByKey(itemData.itemName);
        _itemInforLabel.text = GetInforLabel(itemData);

        switch (itemData.type)
        {
            case ItemData.ItemType.Weapon:
                _itemSkillInfoLabel.text = "Cost : " + itemData.skillCost + "\n";
                _itemSkillInfoLabel.text += "Dmg : " + itemData.skillDmg;
                break;
        }

        _itemValueLabel.text = itemData.value.ToString();
    }

    public void InteractionEventSet(Action action) {
        _interactionEvent += action;
    }

    public void CloseEvent(Action action) {
        _closeEvent = action;
    }

    string GetInforLabel(ItemData item)
    {
        switch (item.type)
        {
            case ItemData.ItemType.Helmet:
                return LangManager.Instance.GetStringByKey("itemInterAction_All");
            case ItemData.ItemType.Armor:
                return LangManager.Instance.GetStringByKey("itemInterAction_HP"); ;
            case ItemData.ItemType.Weapon:
                return LangManager.Instance.GetStringByKey("itemInterAction_Attack"); ;
            case ItemData.ItemType.Shield:
                return LangManager.Instance.GetStringByKey("itemInterAction_Defense");
            default:
                return "";
        }

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
