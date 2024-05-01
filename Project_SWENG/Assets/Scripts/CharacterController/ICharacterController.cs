using CharacterSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICharacterController : IDamagable {

    public void Attack(IList<HexCoordinate> targetPos, int dmg);

    public void UseAttack(int idx);

    public void SetPlay();

    public void ActionEnd();

    public void TurnEnd();

    public void SetActionSelector(IActionSelector actionSelector);

    public void Initial();
}
