using UnityEngine;
using System.Collections.Generic;
using System;

public class GUIUnitInventoryUnit : MonoBehaviour {
    
    [SerializeField] private IGUIUnitItemInfor _itemInfor;
    [SerializeField] private GameObject _light;

    private int _idx;
    private Action<int> _selectedMethod;

    public void SetItemInfor(IList<string> itemList, int idx, Action<int> selectedMethod) {

        DisSelect();

        if (itemList.Count <= idx) {
            gameObject.SetActive(false);
            return;
        }

        gameObject.SetActive(true);

        _idx = idx;

        _itemInfor.SetItemInfor(itemList[idx]);
        _selectedMethod = selectedMethod;

    }

    public void Select()
    {
        _selectedMethod?.Invoke(_idx);
        _light.SetActive(true);
    }

    public void DisSelect()
    {
        _light.SetActive(false);
        
    }

}