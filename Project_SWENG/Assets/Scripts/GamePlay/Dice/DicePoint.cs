[System.Serializable]
public class DicePoint : IDicePoint {
    
    public bool IsRollDice { get; private set; }

    private ICharacterController _cc;
    private int _dicePoint = 0;

    public void SetCC(ICharacterController cc) {
        _cc = cc;
    }

    public void UsePoint(int usingAmount)
    {
        if (_dicePoint < usingAmount)
        {
            return;
        }
        _dicePoint -= usingAmount;
    }

    public int GetPoint() => _dicePoint;

    public void SetPoint(int setValue)
    {
        _dicePoint = setValue;
        IsRollDice = true;
        _cc.ActionEnd(0);
    }

    public void Reset() {
        IsRollDice = false;
    }

}