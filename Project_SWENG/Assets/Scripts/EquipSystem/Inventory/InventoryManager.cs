using System;
using System.Diagnostics;
using System.Xml.Serialization;
using UnityEngine;

public class InventoryManager : MonoSingleton<InventoryManager>
{
    private EquipManager _equipManager;

    private Item _helmet = null;
    private Item _armor  = null;
    public Item Weapon = null;
    private Item _shield = null;

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
                _helmet = item;
                _equipManager.EquipHelmet(item);
                break;
            case Item.ItemType.Armor:
                _armor = item;
                _equipManager.EquipArmor(item);
                break;
            case Item.ItemType.Weapon:
                Weapon = item;
                _equipManager.EquipWeapon(item);
                break;
            case Item.ItemType.Shield:
                _shield = item;
                _equipManager.EquipShield(item);
                break;
        }
    }
        
    
}
