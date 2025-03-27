using UnityEngine;
using UnityEngine.UI;
using EHTool.LangKit;

public class GUIUnitItemInfor : MonoBehaviour
{
    [SerializeField] private Image _icon;
    [SerializeField] private EHText _itemName;
    
    public void SetItemInfor(string itemCode) {
        ItemData data = ItemManager.Instance.GetItemData(itemCode);
    }
}