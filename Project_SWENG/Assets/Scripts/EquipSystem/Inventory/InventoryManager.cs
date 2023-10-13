using System;
using System.Diagnostics;
using System.Xml.Serialization;
using UnityEngine;

public class InventoryManager : MonoSingleton<InventoryManager>
{
    private EquipManager _equipManager;

    private Item Helmet = null;
    private Item Armor  = null;
    public Item Weapon = null;
    private Item Shield = null;

    // EquipManger.Instance,EquipItem -> resetItem + setItem

    public void SetEquipManager(GameObject player)
    {
        _equipManager = player.GetComponent<EquipManager>();
    }

    public void GetItem(Item item)
    {
        if(item.itemHex != null)
            item.itemHex.Item = null;

        GUI_ItemEquiped.Instance.SetItemGUI(item);

        switch (item.type)
        {
            case Item.ItemType.Helmet:
                Helmet = item;
                _equipManager.EquipHelmet(item);
                break;
            case Item.ItemType.Armor:
                Armor = item;
                _equipManager.EquipArmor(item);
                break;
            case Item.ItemType.Weapon:
                Weapon = item;
                _equipManager.EquipWeapon(item);
                break;
            case Item.ItemType.Shield:
                Shield = item;
                _equipManager.EquipShield(item);
                break;
        }
    }
        
    
}
