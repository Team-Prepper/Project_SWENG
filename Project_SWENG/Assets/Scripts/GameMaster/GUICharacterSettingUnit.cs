using EHTool.LangKit;
using EHTool.UIKit;
using System.Collections.Generic;
using UnityEngine;

public class GUICharacterSettingUnit : MonoBehaviour {

    [SerializeField] EHText _name;

    CallbackMethod<string> _characterChangeMethod;
    IList<string> _except;

    internal void SetData(IList<string> except, string characterCode, CallbackMethod<string> characterChangeMethod)
    {
        _except = except;

        gameObject.SetActive(true);

        _characterChangeMethod = characterChangeMethod;
        _name.SetText(characterCode);
    }

    public void CharacterChange() {
        UIManager.Instance.OpenGUI<GUICharacterSelect>("CharacterSelect").Set(_except, (value) => {
            _characterChangeMethod?.Invoke(value);
        });
    }

}
