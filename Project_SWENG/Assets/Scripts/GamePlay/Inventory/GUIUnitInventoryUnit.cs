using UnityEngine;

public class GUIUnitInventoryUnit : MonoBehaviour {
    
    [SerializeField] private GUIUnitItemInfor _itemInfor;

    private Inventory _target;
    private int _idx;

    public void SetItemInfor(Inventory target, int idx) {
        if (target.ItemList.Count >= idx) {
            gameObject.SetActive(false);
            return;
        }
        gameObject.SetActive(true);
        _target = target;
        _idx = idx;
    } 

    public void Use() {
        _target.UseItem(_idx);
    }

}