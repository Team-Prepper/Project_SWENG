namespace SWEng {

    public interface IItemController : IEntity
    {
        public void SetInitial(string itemCode);

        public void Equip();

    }
}