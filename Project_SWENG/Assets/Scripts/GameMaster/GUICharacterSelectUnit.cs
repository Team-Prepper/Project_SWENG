using EHTool.LangKit;
using UnityEngine;

public class GUICharacterSelectUnit : MonoBehaviour {

    [SerializeField] EHText _name;

    string _value;
    CallbackMethod<string> _selectedMethod;

    public void Set(string value, CallbackMethod<string> selectedMethod) {
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