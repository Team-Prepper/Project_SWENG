using System.Diagnostics;
using UnityEngine;
using Debug = System.Diagnostics.Debug;

public class InventoryManager : Singleton<InventoryManager>
{

    private Item helmet = null;
    private Item armor  = null;
    private Item weapon = null;
    private Item shield = null;

    public void GetItem(Item item)
    {
        item.itemHex.Item = null;
        
        GUI_ItemEquiped.Instance.SetItemGUI(item);
        
        switch (item.type)
        {
            case Item.ItemType.Helmet:
                if(helmet == null)
                {
                    int helmetType  = item.id / 100;
                    int helmetIndex = item.id % 100;
                    EquipManager.Instance.EquipHelmet(helmetType, helmetIndex);
                    UnityEngine.Debug.Log("type : " + helmetType + "index : " + helmetIndex);
                    UnityEngine.Debug.Log("EquipHelmet");
                }
                break;
            case Item.ItemType.Armor:
                if (armor == null)
                {
                    EquipManager.Instance.EquipArmor(item.id);
                }
                break; 
            case Item.ItemType.Weapon:
                if (weapon == null)
                {
                    EquipManager.Instance.EquipWeapon(item);
                }
                break;
            case Item.ItemType.Shield:
                if (shield == null)
                {
                    EquipManager.Instance.EquipShield(item);
                }
                break;
        }
    }
}
