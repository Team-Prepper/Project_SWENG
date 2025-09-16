using UnityEngine;
using UnityEngine.UI;
using SWEng.Data;

public class GUIUnitItemDataIcon : GUIUnitItemDataBase
{
    [SerializeField] private Image _icon;
    
    public override void SetItemInfor(string itemCode) {
        ItemData data =
            ItemDataManager.Instance.GetItemData(itemCode);
        Set(data);
    }

    protected virtual void Set(ItemData data) {
        _icon.sprite = data.Icon;

    }
}