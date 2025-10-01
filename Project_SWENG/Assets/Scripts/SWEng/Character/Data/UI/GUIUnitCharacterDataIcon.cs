using UnityEngine;
using UnityEngine.UI;
using SWEng;

public class GUIUnitCharacterDataIcon : GUIUnitCharacterDataBase {

    [SerializeField] private Image _img;
    
    public override void Set(string characterCode) {

        CharacterData data = CharacterDataManager.Instance.GetCharacterData(characterCode);

        Set(data);

    }

    protected virtual void Set(CharacterData data) {

        _img.sprite = data.Image;

    }

}