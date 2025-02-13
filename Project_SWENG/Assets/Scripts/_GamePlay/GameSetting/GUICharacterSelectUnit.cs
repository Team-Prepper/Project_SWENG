using EHTool.LangKit;
using System;
using UnityEngine;

public class GUICharacterSelectUnit : MonoBehaviour {

    [SerializeField] EHText _name;

    string _value;
    Action<string> _selectedMethod;

    public void Set(string value, Action<string> selectedMethod) {
        _value = value;
        _selectedMethod = selectedMethod;
        _name.SetText(value);

        gameObject.SetActive(true);
    }

    public void Select()
    {
        _selectedMethod?.Invoke(_value);
    }

}