using EHTool.LangKit;
using UnityEngine;

public class GUIGameSettingUnit : MonoBehaviour
{

    [SerializeField] EHText _name;

    GUIGameSetting _target;
    string _characterCode;

    internal void SetData(GUIGameSetting target, string characterCode)
    {
        gameObject.SetActive(true);

        _target = target;
        _characterCode = characterCode;
        _name.SetText(characterCode);
    }

    public void Delete() {
        _target.EnemyCharacterRemove(_characterCode);
    }

}
