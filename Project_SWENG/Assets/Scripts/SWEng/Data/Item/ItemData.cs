using UnityEngine;

namespace SWEng.Data {

    [CreateAssetMenu(fileName = "New Item", menuName = "Item/Creat New Item")]
    public class ItemData : ScriptableObject
    {

        public enum ItemTier
        {
            Common,
            UnCommon,
            Rare,
            Unique,
            Legendary,
            Mythic,
        }

        public enum ItemType
        {
            Helmet,
            Armor,
            Weapon,
            Shield,
            Consumables
        }

        [Tooltip("if itemType is helmet : item Code is 100 * helmetTyp + helmetIndex\n" +
                 "helmet Type 0 : headCovering Base\n" +
                 "helmet Type 1 : headCovering No FacialHair\n" +
                 "helmetType 2 : headCovering No Hair\n" +
                 "helmetType 3 : no head\n")]


        public ItemTier tier;
        public ItemType type;

        public string ItemCode;
        public string ItemName;
        public string ItemDesc;
        public string ItemValue;
        public int Cost;

        public Sprite Icon;

        public GameObject Prefab;

    }
}