using UnityEngine;
using UnityEngine.UI;
using EHTool.LangKit;

public class GUIUnitCharacterInforIcon : IGUIUnitCharacterInfor {

    [SerializeField] private Image _img;
    
    public override void Set(string characterCode) {

        CharacterData data = CharacterManager.Instance.GetCharacterData(characterCode);

        Set(data);

    }

    protected virtual void Set(CharacterData data) {

        _img.sprite = data.Image;

    }

}