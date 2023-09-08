using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GUIItemInterAction : GUIPopUp
{
    private Item _targetItem;

    [SerializeField] private TextMeshProUGUI _itemNameLabel;
    [SerializeField] private Text _itemInforLabel;

    protected override void Open()
    {
        base.Open();
    }

    public void SetItem(Item item) {

        _targetItem = item;

        _itemNameLabel.text = item.itemName;

        // id 기반으로 아이템에 대한 설명을 가져오자
        _itemInforLabel.text = item.id.ToString();
    }

    public void InterAction() {
        Debug.Log("InterAction");
    }

}
