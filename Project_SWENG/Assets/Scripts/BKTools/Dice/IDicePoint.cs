namespace BKTools.Gaming.Dice
{
    public interface IDicePoint
    {

        public bool UsePoint(int usingAmount);
        public int GetPoint();
        public void SetPoint(int setValue);

    }
}