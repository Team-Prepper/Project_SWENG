using EHTool.LangKit;
using System;
using UnityEngine;
using UnityEngine.UI;

public class GUIGameSettingUnit : MonoBehaviour
{

    [SerializeField] private Image _img;
    [SerializeField] EHText _name;

    GUIGameSetting _target;
    string _characterCode;

    Action<string> _deleteAction;

    public void SetData(GUIGameSetting target, string characterCode, Action<string> deleteAction)
    {
        gameObject.SetActive(true);

        _target = target;
        _characterCode = characterCode;
        
        _img.sprite = CharacterManager.Instance.GetCharacterData(characterCode).Image;
        _name.SetText(characterCode);

        _deleteAction = deleteAction;
    }

    public void Delete() {
        _deleteAction?.Invoke(_characterCode);
    }

}
