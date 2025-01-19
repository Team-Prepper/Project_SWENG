using EHTool.LangKit;
using UnityEngine;

public class GUIGameSettingUnit : MonoBehaviour
{

    [SerializeField] EHText _name;

    GUIGameSetting _target;
    string _characterCode;

    CallbackMethod<string> _deleteAction;

    internal void SetData(GUIGameSetting target, string characterCode, CallbackMethod<string> deleteAction)
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
