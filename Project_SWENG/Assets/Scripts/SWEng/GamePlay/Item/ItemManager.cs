using System.Collections.Generic;
using UnityEngine;
using EasyH;
using SWEng.Data;

namespace SWEng.GamePlay.Item
{
    public class ItemManager : Singleton<ItemManager>
    {

        public IList<string> ShopItems { get; private set; }

        protected override void OnCreate()
        {
            base.OnCreate();

            ShopItemInitial(5);
        }

        public Item GetItem(string value)
        {
            ItemData data =
                ItemDataManager.Instance.GetItemData(value);
            Item retval = new Item();
            retval.Initial(data.ItemValue);
            return retval;
            
        }

        public void ShopItemInitial(int cnt)
        {
            ShopItems = GetRandomItemList(
                ItemDataManager.Instance.GetItemList(), cnt);
        }
        
        public IList<string> GetRandomItemList(
            ICollection<string> itemKeys, int count)
        {
            List<string> retval = new List<string>();

            int itemCount = itemKeys.Count;
            for (int i = 0; i * itemCount < count - itemCount; i++)
            {
                retval.AddRange(GetRandomSelection(itemKeys, itemCount));
            }
            retval.AddRange(GetRandomSelection(itemKeys, count - retval.Count));
            return retval;
        }

        private IList<T> GetRandomSelection<T>(ICollection<T> originalList, int count)
        {
            IList<T> selection = new List<T>();

            IList<T> tempList = new List<T>(originalList);

            while (selection.Count < count && tempList.Count > 0)
            {
                int randomIndex = Random.Range(0, tempList.Count);
                T selectedItem = tempList[randomIndex];
                selection.Add(selectedItem);
                tempList.RemoveAt(randomIndex);
            }

            return selection;
        }

    }
}