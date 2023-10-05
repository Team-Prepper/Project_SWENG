using System.Diagnostics;
using UnityEngine;
using Debug = System.Diagnostics.Debug;

public class InventoryManager : Singleton<InventoryManager>
{

    public Item Helmet = null;
    public Item Armor  = null;
    public Item Weapon = null;
    public Item Shield = null;
    
    // EquipManger.Instance,EquipItem -> resetItem + setItem
    
    public void GetItem(Item item)
    {
        if(item.itemHex != null)
            item.itemHex.Item = null;
        
        GUI_ItemEquiped.Instance.SetItemGUI(item);
        
        switch (item.type)
        {
            case Item.ItemType.Helmet:
                Helmet = item;
                EquipManager.Instance.EquipHelmet(item);
                break;
            case Item.ItemType.Armor:
                Armor = item;
                EquipManager.Instance.EquipArmor(item);
                break; 
            case Item.ItemType.Weapon:
                Weapon = item;
                EquipManager.Instance.EquipWeapon(item);
                break;
            case Item.ItemType.Shield:
                Shield = item;
                EquipManager.Instance.EquipShield(item);
                break;
        }
    }
}
