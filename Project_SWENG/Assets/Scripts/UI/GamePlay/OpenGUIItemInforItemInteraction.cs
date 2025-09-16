using SWEng.GamePlay;
using EasyH.UI;
using UnityEngine;

[CreateAssetMenu(fileName = "Event_OpenIntemInfor",
    menuName = "Custom/Event/ItemInteraction/GUI", order = 2)]
public class OpenGUIItemInforItemInteraction : ItemInteractionBase
{
    public override void Interaction(ICharacter cc)
    {

        GUI_ItemInterAction ui = UIManager.Instance.
            OpenGUI<GUI_ItemInterAction>("ItemInterAction");

        ui.SetItem(_itemCode);

        ui.InteractionEventSet(() =>
        {
            cc.Inventory.AddItem(_itemCode);
            _interactionEvent?.Invoke();
        });

        ui.CloseEvent(() => { cc.ActionEnd(0); });

    }

}