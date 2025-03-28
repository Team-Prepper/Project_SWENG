using TMPro;
using UnityEngine;
using EHTool.UIKit;
using System.Collections.Generic;
using UnityEngine.UI;

public class GUIShop : GUIPopUp
{
    [SerializeField] int _itemCounts;
    [SerializeField] Text _price;
    [SerializeField] private GUIUnitInventoryUnit[] _units;
    [SerializeField] private IGUIUnitItemInfor _selectedItemInfor;

    private ICharacterController _cc;
    private IList<string> _itemList;

    private int _idx;

    public override void Open()
    {
        base.Open();
        DisplayItem();
    }

    public bool BuyItemToShop(string targetItemCode)
    {
        ItemData targetItem = ItemManager.Instance.GetItemData(targetItemCode);

        if (_cc.DicePoint.GetPoint() >= targetItem.Cost)
        {
            _cc.DicePoint.UsePoint(targetItem.Cost);
            _cc.Inventory.AddItem(targetItemCode);

            _itemList.Remove(targetItemCode);
            return true;
        }

        return false;
    }

    public void SetCC(ICharacterController visitor, MapUnit map) {
        _cc = visitor;
        CameraManager.Instance.CameraSetting(map.transform, "Character");
    }

    private void DisplayItem()
    {
        _itemList = ItemManager.Instance.GetRandomItemList(_itemCounts);

        for (int i = 0; i < _units.Length; i++) {
            _units[i].SetItemInfor(_itemList, i, Select);
        }

        _idx = -1;
        _selectedItemInfor.gameObject.SetActive(false);
    }

    public void Select(int idx) {

        if (_idx >= 0) {
            _units[_idx].DisSelect();
        }
        _idx = idx;
        _selectedItemInfor.gameObject.SetActive(true);
        _selectedItemInfor.SetItemInfor(_itemList[_idx]);
        _price.text = string.Format("$ {0}", ItemManager.Instance.GetItemData(_itemList[_idx]).Cost);

    }

    public void Buy() {
        if (_idx < 0) return;
        BuyItemToShop(_itemList[_idx]);
    }

    public override void Close()
    {
        base.Close();
        _cc.ActionEnd(0);
    }
}
