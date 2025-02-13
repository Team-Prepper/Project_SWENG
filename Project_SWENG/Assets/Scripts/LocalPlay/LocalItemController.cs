using UnityEngine;
using EHTool.UIKit;

public class LocalItemController : MonoBehaviour, IItemController
{
    [SerializeField] private string _itemCode;
    [SerializeField] private float _rotateSpeed = 20;

    [SerializeField] private Transform _itemParentTr;

    public virtual void SetInitial(string itemCode)
    {
        _itemCode = itemCode;

        HexGrid.Instance.GetMapUnitAt(transform.position).SetItem(gameObject, this);

        ItemData data = ItemManager.Instance.GetItemData(itemCode);

        Instantiate(data.itemObject, _itemParentTr);

    }

    public virtual void Interaction(ICharacterController cc)
    {
        GUI_ItemInterAction ui = UIManager.Instance.OpenGUI<GUI_ItemInterAction>("ItemInterAction");

        ui.SetItem(_itemCode);
        ui.InteractionEventSet(() => {
            cc.EquipItem(_itemCode);
            Equip();
        });

        ui.CloseEvent(() =>
        {
            cc.ActionEnd(0);
        });
    }

    public virtual void Equip()
    {
        HexGrid.Instance.GetMapUnitAt(transform.position).ResetEntityState();
        Destroy(gameObject);
    }

    private void Update()
    {
        _itemParentTr.Rotate(Vector3.up * _rotateSpeed * Time.deltaTime);
    }
}
