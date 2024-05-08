using System.Collections.Generic;

public interface ICharacterController : IDamagable {

    public void Initial(string characterName, bool camSync);
    public void SetActionSelector(IActionSelector actionSelector);

    public void SetPlay();
    public void ActionEnd();
    public void TurnEnd();

    public void CamSetting();

    public void Attack(IList<HexCoordinate> targetPos, int dmg, float time);
    public void DoAttack();
    public void DoMove();

    public void MoveTo(HexCoordinate before, HexCoordinate after);

}
