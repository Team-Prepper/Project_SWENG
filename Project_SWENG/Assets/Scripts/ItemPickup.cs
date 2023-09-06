using System;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public Item item;
    private GameObject showUI = null;

    private bool isPickable = false;
    
    private void ShowItem()
    {
        if(showUI == null)
            showUI = ItemUI.Instance.SetPickupItemUI(this);
        showUI.SetActive(true);
        PlayerInputBase.EventItemPickup += PickupHandler;
        ItemUI.Instance.pickableItem.Add(this);
        isPickable = true;
    }

    private void HideItem()
    {
        showUI.SetActive(false); 
        PlayerInputBase.EventItemPickup -= PickupHandler;
        ItemUI.Instance.pickableItem.Remove(this);
        isPickable = false;
    }
    
    public void PickupHandler(object sender, EventArgs e)
    {
        Pickup();
    }

    public void Pickup()
    {
        if(!isPickable) return;
        isPickable = false;
        HideItem();
        InventoryManager.Instance.Add(item);
        ItemUI.Instance.pickableItem.Remove(this);
        DestroyThisObj();
    }

    void DestroyThisObj()
    {
        Destroy(showUI);
        Destroy(gameObject);
    }
}
