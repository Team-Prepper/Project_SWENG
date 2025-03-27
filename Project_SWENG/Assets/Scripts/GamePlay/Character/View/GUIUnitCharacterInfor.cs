using UnityEngine;
using UnityEngine.UI;
using EHTool.LangKit;

public class GUIUnitCharacterInfor : MonoBehaviour {

    [SerializeField] private Image _img;
    [SerializeField] private EHText _name;

    public void Set(string characterCode) {

        CharacterData data = CharacterManager.Instance.GetCharacterData(characterCode);

        _img.sprite = data.Image;
        _name.SetText(data.CharacterName);

    }

}