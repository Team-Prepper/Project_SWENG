namespace SWEng
{
    public class HealItemEffect : IItemEffect
    {
        int _itemValue = 5;

        public HealItemEffect(string value)
        {
            _itemValue = int.Parse(value);
        }

        public void Action(ICharacter cc)
        {
            cc.Status.Heal(_itemValue);
        }

    }
}