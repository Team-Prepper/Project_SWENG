using UnityEngine;
using UnityEngine.UI;
using EHTool.LangKit;
using TMPro;

public class GUIShopUnit : MonoBehaviour
{
    [SerializeField] private Image itemIcon;
    [SerializeField] private Image itemIconBackground;
    [SerializeField] private Text itemName;                 // TextMeshProUGUI -> Text
    [SerializeField] private Image skillIcon;
    
    [SerializeField] private Text itemInfo;                 // TextMeshProUGUI -> Text
    [SerializeField] private TextMeshProUGUI itemCost;

    [SerializeField] private string _itemCode;
    private GUIShop _shop;

    public void SetItem(GUIShop shop, string itemCode)
    {
        _shop = shop;
        _itemCode = itemCode;

        ItemData item = ItemManager.Instance.GetItemData(itemCode);

        itemIcon.sprite = item.Icon;
        itemIconBackground.color = ItemManager.Instance.GetTierColor(item.tier);
        //skillIcon.sprite = item.skillIcon;
        itemName.text = LangManager.Instance.GetStringByKey(item.ItemName);
        itemCost.text = item.Cost.ToString();
        
    }

    private string GetString(Item.ItemType itemType) {

        switch (itemType)
        {
            case Item.ItemType.Helmet:
                return LangManager.Instance.GetStringByKey("shopItem_All");
            case Item.ItemType.Armor:
                return LangManager.Instance.GetStringByKey("shopItem_HP");
            case Item.ItemType.Weapon:
                return LangManager.Instance.GetStringByKey("shopItem_Attack");
            case Item.ItemType.Shield:
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
