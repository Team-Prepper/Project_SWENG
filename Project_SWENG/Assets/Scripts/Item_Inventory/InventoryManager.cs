using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;
    public List<Item> items = new List<Item>();
    public List<ItemSlot> slots = new List<ItemSlot>();
    public List<GameObject> EquipmentSlots;
    private List<Color> colors = new List<Color>();
    public int slotNum;
    
    public Transform itemContent;
    public GameObject itemSlot;
    public GameObject detailPanel;

    public Dictionary<ItemSlot, Item> inventory = new Dictionary<ItemSlot, Item>();

    private void Awake()
    {
        Instance = this;
        SetColorList();
        SetSlot();
        detailPanel.SetActive(false);
    }
    
    private void SetColorList()
    {
        colors.Add(new Color(0.7169812f,0.5083325f,0.01690993f,1f)); // common
        colors.Add(new Color(0.2722067f,0.5849056f,0.13519505f,1f)); // uncommom
        colors.Add(new Color(0.1541919f,0.3933419f,0.71223475f,1f)); //rare
        colors.Add(new Color(0.4543215f,0.2126654f,0.99174132f,1f)); //epic
        colors.Add(new Color(0.8971235f,0.8946123f,0.21643756f,1f)); //legendary
        colors.Add(new Color(0.9912354f,0.3451256f,0.61234353f,1f)); //mythic
    }

    private void SetSlot()
    {
        foreach (GameObject slot in EquipmentSlots)
        {
            SetSlotDefault(slot);
        }
    }

    public void Add(Item item)
    {
        items.Add(item);
        GetItem(item);
    }

    public void Remove(Item item)
    {
        items.Remove(item);
    }
    
    public void GetItem(Item item)
    {
        GameObject slotItem = Instantiate(itemSlot, itemContent);
        slotItem.GetComponent<DraggableUI>().itemData = item;
        slotItem.transform.Find("Icon").GetComponent<Image>().sprite = item.icon;
        
        SetSlotDefault(slotItem);
    }

    public void SetSlotDefault(GameObject slot)
    {
        slot.transform.Find("Equipped")?.gameObject.SetActive(false);
        slot.transform.Find("Cooldown")?.gameObject.SetActive(false);
        slot.transform.Find("Selection")?.gameObject.SetActive(false);
        slot.transform.Find("Count")?.gameObject.SetActive(false);
    }
}
