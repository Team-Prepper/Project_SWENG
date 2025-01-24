using EHTool.LangKit;
using UnityEngine;
using System;

public class GUIGameSettingUnit : MonoBehaviour
{

    [SerializeField] EHText _name;

    GUIGameSetting _target;
    string _characterCode;

    Action<string> _deleteAction;

    public void SetData(GUIGameSetting target, string characterCode, Action<string> deleteAction)
    {
        gameObject.SetActive(true);

        _target = target;
        _characterCode = characterCode;
        _name.SetText(characterCode);

        _deleteAction = deleteAction;
    }

    public void Delete() {
        _deleteAction?.Invoke(_characterCode);
    }

}
