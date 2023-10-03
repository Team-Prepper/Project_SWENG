using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GUI_ItemInterAction : GUIPopUp
{
    private Item _targetItem;

    [SerializeField] private TextMeshProUGUI _itemNameLabel;
    [SerializeField] private TextMeshProUGUI _itemInforLabel;

    protected override void Open(Vector2 openPos)
    {
        base.Open(new Vector2(800,0));
    }

    public void SetItem(Item item) {

        _targetItem = item;

        _itemNameLabel.text = item.itemName;
        _itemInforLabel.text = item.id.ToString();

    }

    public void InterAction() {
        InventoryManager.Instance.GetItem(_targetItem);
    }

}
