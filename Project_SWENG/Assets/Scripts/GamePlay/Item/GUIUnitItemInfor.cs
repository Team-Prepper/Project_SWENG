using UnityEngine;
using EHTool.LangKit;

public class GUIUnitItemInfor : GUIUnitItemInforIcon
{
    [SerializeField] private EHText _itemName;
    [SerializeField] private EHText _itemDesc;
    
    public override void SetItemInfor(string itemCode) {

        base.SetItemInfor(itemCode);

        ItemData data = ItemManager.Instance.GetItemData(itemCode);
        _itemName.text = data.ItemName;
        _itemDesc.text = data.ItemDesc;
    }
}