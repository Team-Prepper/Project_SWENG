using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using static Unity.VisualScripting.Antlr3.Runtime.Tree.TreeWizard;

public class ShopManager : MonoSingleton<ShopManager>
{
    public GameObject GUI_ShopPrefab;
    private GUI_ShopInterAction GUI_shop;
    public List<Item> items = new List<Item>();
    private List<Item> selectedList = new List<Item>();

    private DicePoint visitor;

    public void WelcomeToShop(GameObject player)
    {
        if (player.GetPhotonView().IsMine == true)
        {
            visitor = player.GetComponent<DicePoint>();
            if(visitor != null )
            {
                GUI_shop = Instantiate(GUI_ShopPrefab).GetComponent<GUI_ShopInterAction>();
            }
        }
    }

    public bool BuyItemToShop(Item targetItem)
    {
        if(visitor.GetPoint() >= targetItem.cost)
        {
            visitor.UsePoint(targetItem.cost);
            InventoryManager.Instance.GetItem(targetItem);
            GUI_shop.showComment("Thank you for your purchase");
            return true;
        }

        GUI_shop.showComment("Not enough DicePoint");
        return false;
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
