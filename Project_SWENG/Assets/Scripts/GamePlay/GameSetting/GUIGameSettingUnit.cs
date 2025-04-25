using System;
using UnityEngine;

public class GUIGameSettingUnit : MonoBehaviour
{

    [SerializeField] private GUIUnitCharacterInforIcon _characterInfor;

    private string _characterCode;

    private Action<string> _deleteAction;

    public void SetData(string characterCode, Action<string> deleteAction)
    {
        gameObject.SetActive(true);

        _characterCode = characterCode;
        _characterInfor.Set(characterCode);

        _deleteAction = deleteAction;
    }

    public void Delete() {
        _deleteAction?.Invoke(_characterCode);
    }

}
