using UnityEngine;
using EHTool.LangKit;

public class GUIUnitCharacterInfor : GUIUnitCharacterInforIcon {
    
    [SerializeField] private EHText _name;
    [SerializeField] private EHText _desc;

    public override void Set(string characterCode) {

        base.Set(characterCode);

        CharacterData data = CharacterManager.Instance.GetCharacterData(characterCode);

        _name.SetText(data.CharacterName);

    }

}