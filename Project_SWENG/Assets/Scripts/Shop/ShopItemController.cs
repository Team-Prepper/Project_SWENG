using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class ShopItemController : MonoBehaviour
{
    [SerializeField] Image itemIcon;
    [SerializeField] Image itemIconBackground;
    [SerializeField] TextMeshProUGUI itemName;
    [SerializeField] Image skillIcon;
                                        [SerializeField] TextMeshProUGUI itemInfo;
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
        itemName.text = item.name;
        itemCost.text = item.cost.ToString();

        switch (item.type)
        {
            case Item.ItemType.Helmet:
                itemInfo.text = "ALL : ";
                break;
            case Item.ItemType.Armor:
                itemInfo.text = "HP : ";
                break;
            case Item.ItemType.Weapon:
                itemInfo.text = "ATK : ";
                break;
            case Item.ItemType.Shield:
                itemInfo.text = "DEF : ";
                break;
            default:
                itemInfo.text = "";
                break;
        }
        itemInfo.text += item.value.ToString();
    }

    public void BuyItemHandler()
    {
        ShopManager.Instance.BuyItemToShop(thisItem);
        Destroy(this.gameObject);
    }
}
