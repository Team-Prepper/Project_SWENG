using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UISystem;

public class GUI_ShopInterAction : GUIPopUp
{
    public int itemCounts;
    [SerializeField] Transform itemSlot;
    [SerializeField] GameObject itemInfo;
    [SerializeField] TextMeshProUGUI shopComment;
    protected override void Open(Vector2 openPos)
    {
        base.Open(new Vector2(-1000,0));
        CamMovement.Instance.IsPlayerMove = true;
        DisplayItem();
    }

    private void DisplayItem()
    {
        foreach (Item item in ShopManager.Instance.GetRandomItemList(itemCounts))
        {
            SetItem(item);
        }

    }

    public void ExitShop()
    {
        CamMovement.Instance.IsPlayerMove = false;
        Destroy(gameObject);
    }

    private void SetItem(Item item)
    {
        GameObject itemInstance = Instantiate(itemInfo, itemSlot);
        itemInstance.GetComponent<ShopItemController>().SetItem(item);
    }

    public void showComment(string comment)
    {
        shopComment.text = comment;
    }
}
