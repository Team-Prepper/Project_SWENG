using UnityEngine;
using EasyH.Tool.LangKit;
using SWEng.Data;

public class GUIUnitCharacterDataName : GUIUnitCharacterDataIcon
{

    [SerializeField] private EHText _name;

    protected override void Set(CharacterData data)
    {

        base.Set(data);

        _name.SetText(data.CharacterName);

    }

}