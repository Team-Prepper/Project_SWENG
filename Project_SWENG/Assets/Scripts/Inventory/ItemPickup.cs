using System;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public Item item;
    private GameObject showUI = null;

    private void ShowItem()
    {
        if(showUI == null)
            showUI = ItemUI.Instance.SetPickupItemUI(this);
        showUI.SetActive(true);
    }

    private void HideItem()
    {
        showUI.SetActive(false); 
    }
    
    public void PickupHandler()
    {
        Pickup();
    }

    public void Pickup()
    {
        HideItem();
        InventoryManager.Instance.GetItem(item);
        DestroyThisObj();
    }

    void DestroyThisObj()
    {
        Destroy(showUI);
        Destroy(gameObject);
    }
}
