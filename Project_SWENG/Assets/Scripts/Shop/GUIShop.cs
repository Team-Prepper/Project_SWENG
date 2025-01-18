using TMPro;
using UnityEngine;
using EHTool.UIKit;

public class GUIShop : GUIPopUp
{
    [SerializeField] int _itemCounts;

    [SerializeField] GUIShopUnit[] _itemInfo;
    [SerializeField] TextMeshProUGUI _shopComment;

    ICharacterController _cc;

    public override void Open()
    {
        base.Open();
        DisplayItem();
    }

    public bool BuyItemToShop(string targetItemCode)
    {
        ItemData targetItem = ItemManager.Instance.GetItemData(targetItemCode);

        if (_cc.Character.GetPoint() >= targetItem.cost)
        {
            _cc.Character.UsePoint(targetItem.cost);
            _cc.EquipItem(targetItemCode);
            showComment("Thank you for your purchase");
            return true;
        }

        showComment("Not enough DicePoint");
        return false;
    }

    public void SetCC(ICharacterController visitor, MapUnit map) {
        _cc = visitor;
        CameraManager.Instance.CameraSetting(map.transform, "Character");
    }

    private void DisplayItem()
    {
        int i = 0;

        foreach (string itemCode in ItemManager.Instance.GetRandomItemList(_itemCounts))
        {
            _itemInfo[i++].SetItem(this, itemCode);
        }

    }

    public void showComment(string comment)
    {
        _shopComment.text = comment;
    }

    public override void Close()
    {
        base.Close();
        _cc.ActionEnd(0);
    }
}
