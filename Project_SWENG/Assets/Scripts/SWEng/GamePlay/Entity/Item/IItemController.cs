namespace SWEng.GamePlay {

    public interface IItemController : IEntity
    {
        public void SetInitial(string itemCode);

        public void Equip();

    }
}