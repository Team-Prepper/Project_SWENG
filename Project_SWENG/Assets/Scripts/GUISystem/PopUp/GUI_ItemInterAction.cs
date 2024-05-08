using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using LangSystem;
using UISystem;

public class GUI_ItemInterAction : GUIPopUp
{
    private Item _targetItem;

    [SerializeField] private Image _itemIcon;
    [SerializeField] private Image _itemSkillIcon;
    [SerializeField] private Text _itemNameLabel;
    [SerializeField] private Text _itemInforLabel;
    [SerializeField] private Text _itemValueLabel;
    [SerializeField] private TextMeshProUGUI _itemSkillInfoLabel;
    protected override void Open(Vector2 openPos)
    {
        base.Open(new Vector2(800,0));
    }

    public void SetItem(Item item) {

        _targetItem = item;
        _itemIcon.sprite = item.icon;
        _itemSkillIcon.sprite = item.skillIcon;

        _itemNameLabel.text = StringManager.Instance.GetStringByKey(item.itemName);

        switch (item.type)
        {
            case Item.ItemType.Helmet:
                _itemInforLabel.text = StringManager.Instance.GetStringByKey("itemInterAction_All");
                break;
            case Item.ItemType.Armor:
                _itemInforLabel.text = StringManager.Instance.GetStringByKey("itemInterAction_HP"); ;
                break;
            case Item.ItemType.Weapon:
                _itemInforLabel.text = StringManager.Instance.GetStringByKey("itemInterAction_Attack"); ;
                _itemSkillInfoLabel.text = "Cost : " + item.skillCost + "\n";
                _itemSkillInfoLabel.text += "Dmg : " + item.skillDmg;
                break;
            case Item.ItemType.Shield:
                _itemInforLabel.text = StringManager.Instance.GetStringByKey("itemInterAction_Defense"); ;
                break;
            default:
                _itemInforLabel.text = "";
                break;
        }
        _itemValueLabel.text = item.value.ToString();

    }

    public void InterAction() {
        InventoryManager.Instance.GetItem(_targetItem);
    }

}
