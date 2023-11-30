using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LangSystem;
using TMPro;
using Unity.VisualScripting;

public class ShopItemController : MonoBehaviour
{
    [SerializeField] Image itemIcon;
    [SerializeField] Image itemIconBackground;
    [SerializeField] Text itemName;                 // TextMeshProUGUI -> Text
    [SerializeField] Image skillIcon;
    
    [SerializeField] Text itemInfo;                 // TextMeshProUGUI -> Text
    [SerializeField] TextMeshProUGUI itemCost;
                                    
    private List<Color> colors = new List<Color>();
    [SerializeField] private Item thisItem;

    private void OnEnable()
    {
        SetColorList();
    }

    private void SetColorList()
    {
        colors.Add(new Color(0.7169812f, 0.5083325f, 0.01690993f, 1f)); // common
        colors.Add(new Color(0.2722067f, 0.5849056f, 0.13519505f, 1f)); // uncommon
        colors.Add(new Color(0.1541919f, 0.3933419f, 0.71223475f, 1f)); // rare
        colors.Add(new Color(0.4543215f, 0.2126654f, 0.99174132f, 1f)); // epic
        colors.Add(new Color(0.8971235f, 0.8946123f, 0.21643756f, 1f)); // legendary
        colors.Add(new Color(0.9912354f, 0.3451256f, 0.61234353f, 1f)); // mythic
    }

    public void SetItem(Item item)
    {
        thisItem = item;
        itemIcon.sprite = item.icon;
        itemIconBackground.color = colors[(int)item.tier];
        skillIcon.sprite = item.skillIcon;
        itemName.text = StringManager.Instance.GetStringByKey(item.itemName);
        itemCost.text = item.cost.ToString();

        string format;

        switch (item.type)
        {
            case Item.ItemType.Helmet:
                format = StringManager.Instance.GetStringByKey("shopItem_All");
                break;
            case Item.ItemType.Armor:
                format = StringManager.Instance.GetStringByKey("shopItem_HP");
                break;
            case Item.ItemType.Weapon:
                format = StringManager.Instance.GetStringByKey("shopItem_Attack");
                break;
            case Item.ItemType.Shield:
                format = StringManager.Instance.GetStringByKey("shopItem_Defense");
                break;
            default:
                return;
        }
        itemInfo.text = string.Format(format, item.value);
    }

    public void BuyItemHandler()
    {
        if(ShopManager.Instance.BuyItemToShop(thisItem))
            Destroy(this.gameObject);
    }
}
