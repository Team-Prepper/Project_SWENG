using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoSingleton<ShopManager>
{
    public GameObject GUI_item;
    public List<Item> items = new List<Item>();
    private List<Item> selectedList = new List<Item>();

    public void WelcomeToShop()
    {
        Instantiate(GUI_item);
    }

    public void BuyItemToShop(Item targetItem)
    {
        InventoryManager.Instance.GetItem(targetItem);
    }

    public List<Item> GetRandomItemList(int itemCounts)
    {
        return GetRandomSelection(items, itemCounts);
    }

    private List<T> GetRandomSelection<T>(List<T> originalList, int count)
    {
        List<T> selection = new List<T>();

        List<T> tempList = new List<T>(originalList);

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
