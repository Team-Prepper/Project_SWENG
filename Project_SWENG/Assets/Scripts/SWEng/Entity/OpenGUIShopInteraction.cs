using UnityEngine;
using SWEng;
using EasyH.Unity.UI;

[CreateAssetMenu(fileName = "Event_OpenShop",
    menuName = "Custom/Event/ShopInteraction/GUI", order = 2)]
public class OpenGUIShopInteraction : ShopInteractionBase
{
    public override void Interaction(ICharacter actor)
    {
        UIManager.Instance.OpenGUI<GUIShop>(
            "Shop").SetCC(actor, _mapUnit);
    }
}