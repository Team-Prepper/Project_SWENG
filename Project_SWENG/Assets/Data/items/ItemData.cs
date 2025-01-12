using UnityEngine;
using EHTool.UIKit;

[CreateAssetMenu(fileName = "New Item", menuName = "Item/Creat New Item")]
public class ItemData : ScriptableObject
{
    [Tooltip("if itemType is helmet : item Code is 100 * helmetTyp + helmetIndex\n" +
             "helmet Type 0 : headCovering Base\n" + 
             "helmet Type 1 : headCovering No FacialHair\n" +
             "helmetType 2 : headCovering No Hair\n" +
             "helmetType 3 : no head\n")]

    public enum ItemTier {
        Common,
        UnCommon,
        Rare,
        Unique,
        Legendary,
        Mythic,
    }

    public enum ItemType {
        Helmet,
        Armor,
        Weapon,
        Shield,
        Consumables
    }

    public int id;
    public bool isEquip = false;
    
    public string itemName;
    public int value;
    public int cost;
    public Sprite icon;
    public Sprite skillIcon;
    public bool hasSkill = false;
    public int skillCost;
    public int skillDmg;
    public GameObject itemObject;
    public MapUnit itemHex; // Required to remove dropped items


    public ItemTier tier;

    public ItemType type;
    
    public void Pick() {
        UIManager.Instance.OpenGUI<GUI_ItemInterAction>("ItemInterAction").SetItem(this);    
    }
}