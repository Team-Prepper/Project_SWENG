namespace SWEng.GamePlay {

    public class DiceItemEffect : IItemEffect
    {

        public DiceItemEffect(string value)
        {

        }

        public void Action(ICharacter cc)
        {
            cc.IsRollDice = false;
        }

    }
}