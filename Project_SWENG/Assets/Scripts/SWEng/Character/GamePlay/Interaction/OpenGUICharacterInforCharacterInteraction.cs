using UnityEngine;
using EasyH.Unity.UI;
using SWEng;

[CreateAssetMenu(fileName = "Event_OpenCharacterInfor",
    menuName = "Custom/Event/CharacterInteraction/GUI", order = 2)]
public class OpenGUICharacterInforCharacterInteraction : CharacterInteractionBase
{
    public override void Interaction(ICharacter actor)
    {
        UIManager.Instance.OpenGUI<GUI_ShowCharacterInfor>(
            "CharacterInfor").SetInfor(_target, actor);
    }
}