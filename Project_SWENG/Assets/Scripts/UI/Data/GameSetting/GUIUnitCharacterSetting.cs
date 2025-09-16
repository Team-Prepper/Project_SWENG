using EasyH.Tool.LangKit;
using EasyH.UI;
using System;
using System.Collections.Generic;
using UnityEngine;
using SWEng.Data;

public class GUIUnitCharacterSetting : MonoBehaviour {

    [SerializeField] private EHText _name;

    private Action<string> _characterChangeMethod;
    private IList<string> _except;

    public void SetData(IList<string> except, string characterCode, Action<string> characterChangeMethod)
    {
        _except = except;

        gameObject.SetActive(true);

        _characterChangeMethod = characterChangeMethod;
        
        _name.SetText(CharacterDataManager.Instance.
            GetCharacterData(characterCode).CharacterName);
    }

    public void CharacterChange() {
        UIManager.Instance.OpenGUI<GUICharacterSelect>("CharacterSelect").Set(_except, (value) => {
            _characterChangeMethod?.Invoke(value);
        });
    }

}
