using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EHTool.LangKit;
using TMPro;

public class GUIShopUnit : MonoBehaviour
{
    [SerializeField] Image itemIcon;
    [SerializeField] Image itemIconBackground;
    [SerializeField] Text itemName;                 // TextMeshProUGUI -> Text
    [SerializeField] Image skillIcon;
    
    [SerializeField] Text itemInfo;                 // TextMeshProUGUI -> Text
    [SerializeField] TextMeshProUGUI itemCost;

    [SerializeField] private string _itemCode;
    private GUIShop _shop;

    public void SetItem(GUIShop shop, string itemCode)
    {
        _shop = shop;
        _itemCode = itemCode;

        ItemData item = ItemManager.Instance.GetItemData(itemCode);

        itemIcon.sprite = item.icon;
        itemIconBackground.color = ItemManager.Instance.GetTierColor(item.tier);
        skillIcon.sprite = item.skillIcon;
        itemName.text = LangManager.Instance.GetStringByKey(item.itemName);
        itemCost.text = item.cost.ToString();

        itemInfo.text = string.Format(GetString(item.type), item.value);
    }

    private string GetString(ItemData.ItemType itemType) {

        switch (itemType)
        {
            case ItemData.ItemType.Helmet:
                return LangManager.Instance.GetStringByKey("shopItem_All");
            case ItemData.ItemType.Armor:
                return LangManager.Instance.GetStringByKey("shopItem_HP");
            case ItemData.ItemType.Weapon:
                return LangManager.Instance.GetStringByKey("shopItem_Attack");
            case ItemData.ItemType.Shield:
                return LangManager.Instance.GetStringByKey("shopItem_Defense");
            default:
                return "{0}";
        }
    }

    public void BuyItemHandler()
    {
        if (_shop.BuyItemToShop(_itemCode))
            gameObject.SetActive(false);
    }
}
