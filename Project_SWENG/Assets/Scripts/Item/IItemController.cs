public interface IItemController
{
    public void SetInitial(string itemCode);

    public void Interaction(ICharacterController cc);

    public void Equip();

}