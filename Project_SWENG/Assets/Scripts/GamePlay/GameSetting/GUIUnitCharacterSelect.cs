using System;
using System.Collections.Generic;
using UnityEngine;

public class GUIUnitCharacterSelect : MonoBehaviour {

    [SerializeField] private IGUIUnitCharacterInfor _characterInfor;
    [SerializeField] private GameObject _light;

    private int _idx;
    private Action<int> _selectedMethod;

    public void Set(IList<string> characterList, int idx, Action<int> selectedMethod) {
        if (idx >= characterList.Count) {
            gameObject.SetActive(false);
            return;
        }
        _idx = idx;
        _selectedMethod = selectedMethod;

        _characterInfor.Set(characterList[idx]);

        gameObject.SetActive(true);
        DisSelect();
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