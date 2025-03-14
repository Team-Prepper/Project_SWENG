using EHTool.LangKit;
using System;
using UnityEngine;
using UnityEngine.UI;

public class GUICharacterSelectUnit : MonoBehaviour {

    [SerializeField] private Image _img;
    [SerializeField] private EHText _name;

    private string _value;
    private Action<string> _selectedMethod;

    public void Set(string value, Action<string> selectedMethod) {
        _value = value;
        _selectedMethod = selectedMethod;

        _img.sprite = CharacterManager.Instance.GetCharacterData(value).Image;
        _name.SetText(value);

        gameObject.SetActive(true);
    }

    public void Select()
    {
        Debug.Log(_value);
        _selectedMethod?.Invoke(_value);
    }

}