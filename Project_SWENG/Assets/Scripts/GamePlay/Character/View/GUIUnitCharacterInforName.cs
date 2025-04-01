using UnityEngine;
using EHTool.LangKit;

public class GUIUnitCharacterInforName : GUIUnitCharacterInforIcon {
    
    [SerializeField] private EHText _name;

    protected override void Set(CharacterData data) {

        base.Set(data);

        _name.SetText(data.CharacterName);

    }

}