using System.Collections.Generic;

public interface IActionSelector
{
    public enum Action {
        Dice, Move, Attack, TurnEnd,
        Interaction
    }

    public void SetCharacterController(ICharacterController cc);
    public void Ready(IList<Action> actionList);

    public void Die();

}
