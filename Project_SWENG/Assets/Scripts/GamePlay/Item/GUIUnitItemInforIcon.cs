using UnityEngine;
using UnityEngine.UI;
using EHTool.LangKit;

public class GUIUnitItemInforIcon : IGUIUnitItemInfor
{
    [SerializeField] private Image _icon;
    
    public override void SetItemInfor(string itemCode) {
        ItemData data = ItemManager.Instance.GetItemData(itemCode);
        Set(data);
    }

    protected virtual void Set(ItemData data) {
        _icon.sprite = data.Icon;

    }
}