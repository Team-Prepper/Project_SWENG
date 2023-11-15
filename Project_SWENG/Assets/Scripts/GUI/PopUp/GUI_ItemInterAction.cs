using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
using UISystem;

public class GUI_ItemInterAction : GUIPopUp
{
    private Item _targetItem;

    [SerializeField] private Image _itemIcon;
    [SerializeField] private Image _itemSkillIcon;
    [SerializeField] private TextMeshProUGUI _itemNameLabel;
    [SerializeField] private TextMeshProUGUI _itemInforLabel;
    [SerializeField] private TextMeshProUGUI _itemSkillInfoLabel;
    protected override void Open(Vector2 openPos)
    {
        base.Open(new Vector2(800,0));
    }

    public void SetItem(Item item) {

        _targetItem = item;
        _itemIcon.sprite = item.icon;
        _itemSkillIcon.sprite = item.skillIcon;
        _itemNameLabel.text = item.itemName;
        _itemSkillInfoLabel.text = "";
        switch (item.type)
        {
            case Item.ItemType.Helmet:
                _itemInforLabel.text = "ALL : ";
                break;
            case Item.ItemType.Armor:
                _itemInforLabel.text = "HP : ";
                break;
            case Item.ItemType.Weapon:
                _itemInforLabel.text = "ATK : ";
                _itemSkillInfoLabel.text = "Cost : " + item.skillCost + "\n";
                _itemSkillInfoLabel.text += "Dmg : " + item.skillDmg;
                break;
            case Item.ItemType.Shield:
                _itemInforLabel.text = "DEF : ";
                break;
            default:
                _itemInforLabel.text = "";
                break;
        }
        _itemInforLabel.text += item.value.ToString();

    }

    public void InterAction() {
        InventoryManager.Instance.GetItem(_targetItem);
    }

}
