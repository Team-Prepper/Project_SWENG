using System;
using UnityEngine;

public class GUIUnitCharacterSelect : MonoBehaviour {

    [SerializeField] private GUIUnitCharacterInfor _characterInfor;
    private string _value;
    private Action<string> _selectedMethod;

    public void Set(string characterCode, Action<string> selectedMethod) {
        _value = characterCode;
        _selectedMethod = selectedMethod;

        _characterInfor.Set(characterCode);

        gameObject.SetActive(true);
    }

    public void Select()
    {
        _selectedMethod?.Invoke(_value);
    }

}