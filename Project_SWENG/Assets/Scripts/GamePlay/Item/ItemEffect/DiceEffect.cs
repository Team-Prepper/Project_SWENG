public class DiceItemEffect : IItemEffect
{

    public DiceItemEffect(string value) {
        
    }

    public void Action(ICharacterController cc) {
        cc.IsRollDice = false;
    }

}