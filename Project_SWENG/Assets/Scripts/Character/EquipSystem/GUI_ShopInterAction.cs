using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using EHTool.UIKit;

public class GUI_ShopInterAction : GUIPopUp
{
    public int itemCounts;
    [SerializeField] Transform itemSlot;
    [SerializeField] GameObject itemInfo;
    [SerializeField] TextMeshProUGUI shopComment;

    public override void Open()
    {
        base.Open();
        CameraManager.Instance.IsPlayerMove = true;
        DisplayItem();
    }

    private void DisplayItem()
    {
        foreach (ItemData item in ShopManager.Instance.GetRandomItemList(itemCounts))
        {
            SetItem(item);
        }

    }

    public void ExitShop()
    {
        CameraManager.Instance.IsPlayerMove = false;
        Destroy(gameObject);
    }

    private void SetItem(ItemData item)
    {
        GameObject itemInstance = Instantiate(itemInfo, itemSlot);
        itemInstance.GetComponent<ShopItemController>().SetItem(item);
    }

    public void showComment(string comment)
    {
        shopComment.text = comment;
    }
}
