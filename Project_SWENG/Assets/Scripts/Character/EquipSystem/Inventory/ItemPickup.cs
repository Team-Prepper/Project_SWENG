using System;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public ItemData item;
    private GameObject showUI = null;

    private void ShowItem()
    {
        if(showUI == null)
            showUI = GUIItemUI.Instance.SetPickupItemUI(this);
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
