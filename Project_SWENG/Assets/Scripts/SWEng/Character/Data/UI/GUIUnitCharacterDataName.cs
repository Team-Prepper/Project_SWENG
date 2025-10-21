using UnityEngine;
using EasyH.Unity.LangKit;
using SWEng;

public class GUIUnitCharacterDataName : GUIUnitCharacterDataIcon
{

    [SerializeField] private EHText _name;

    protected override void Set(CharacterData data)
    {

        base.Set(data);

        _name.SetText(data.CharacterName);

    }

}