using EHTool.LangKit;
using System;
using UnityEngine;
using UnityEngine.UI;

public class GUICharacterSelectUnit : MonoBehaviour {

    [SerializeField] private Image _img;
    [SerializeField] private EHText _name;

    private string _value;
    private Action<string> _selectedMethod;

    public void Set(string characterCode, Action<string> selectedMethod) {
        _value = characterCode;
        _selectedMethod = selectedMethod;

        CharacterData data = CharacterManager.Instance.GetCharacterData(characterCode);

        _img.sprite = data.Image;
        _name.SetText(data.CharacterName);

        gameObject.SetActive(true);
    }

    public void Select()
    {
        _selectedMethod?.Invoke(_value);
    }

}