public class HealEffect : IItemEffect
{
    int _itemValue = 5;

    public HealEffect(string value) {
        _itemValue = int.Parse(value);
    }

    public void Action(ICharacterController cc) {
        cc.Status.TakeDamage(-_itemValue);
    }

}
