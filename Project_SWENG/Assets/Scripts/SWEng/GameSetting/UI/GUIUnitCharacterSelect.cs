using System;
using System.Collections.Generic;
using UnityEngine;
using SWEng;

public class GUIUnitCharacterSelect : MonoBehaviour {

    [SerializeField] private GUIUnitCharacterDataBase _characterData;
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

        _characterData.Set(characterList[idx]);

        gameObject.SetActive(true);
        SetLightActive(false);
    }

    public void Select()
    {
        _selectedMethod?.Invoke(_idx);
    }

    public void SetLightActive(bool isActive)
    {
        _light.SetActive(isActive);
        
    }

}