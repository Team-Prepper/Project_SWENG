using CharacterSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICharacterController : IDamagable {

    public void Initial();
    public void SetActionSelector(IActionSelector actionSelector);

    public void SetPlay();
    public void ActionEnd();
    public void TurnEnd();

    public void Attack(IList<HexCoordinate> targetPos, int dmg);
    public void UseAttack(int idx);

    public void MoveTo(HexCoordinate before, HexCoordinate after);

}
