using System.Collections.Generic;

public interface IActionSelector
{
    public enum Action {
        Dice, Move, Attack, TurnEnd,
        Interaction, Inventory
    }

    public void Ready(ICharacterController cc, IList<Action> actionList);
    public void SelectTarget(ISkill attack);

}
