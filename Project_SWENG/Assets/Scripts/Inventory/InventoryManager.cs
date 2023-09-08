public class InventoryManager : Singleton<InventoryManager>
{

    private Item helmet = null;
    private Item armor  = null;
    private Item weapon = null;
    private Item shield = null;

    public void GetItem(Item item)
    {
        switch (item.type)
        {
            case Item.ItemType.Helmet:
                if(helmet == null)
                {
                    int helmetType  = item.id / 100;
                    int helmetIndex = item.id % 100;
                    EquipManager.Instance.EquipHelmet(helmetType, helmetIndex);
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

                }
                break;
            case Item.ItemType.Shield:
                if (shield == null)
                {

                }
                break;
        }
    }
}
