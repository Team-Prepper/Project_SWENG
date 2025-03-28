using UnityEngine;
using UnityEngine.UI;
using EHTool.LangKit;

public class GUIUnitCharacterInforIcon : IGUIUnitCharacterInfor {

    [SerializeField] private Image _img;
    public override void Set(string characterCode) {

        CharacterData data = CharacterManager.Instance.GetCharacterData(characterCode);

        _img.sprite = data.Image;

    }

}