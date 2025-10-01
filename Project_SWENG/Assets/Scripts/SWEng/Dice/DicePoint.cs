
namespace SWEng
{

    [System.Serializable]
    public class DicePoint : IDicePoint
    {

        private int _dicePoint = 0;

        public bool UsePoint(int usingAmount)
        {
            if (_dicePoint < usingAmount)
            {
                return false;
            }
            _dicePoint -= usingAmount;
            return true;
        }

        public int GetPoint() => _dicePoint;

        public void SetPoint(int setValue)
        {
            _dicePoint = setValue;
        }

    }
}