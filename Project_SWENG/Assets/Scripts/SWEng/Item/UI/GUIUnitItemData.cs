using UnityEngine;
using EasyH.Unity.LangKit;
using SWEng;

public class GUIUnitItemData : GUIUnitItemDataIcon
{
    [SerializeField] private EHText _itemName;
    [SerializeField] private EHText _itemDesc;
    
    protected override void Set(ItemData data) {

        base.Set(data);

        _itemName.SetText(data.ItemName);
        _itemDesc.SetText(data.ItemDesc);

    }
}