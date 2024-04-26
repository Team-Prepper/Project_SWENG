using CharacterSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICharacterController {

    public void TakeDamage(int amount);

    public void Attack(Vector3 targetPos, bool isSkill = false);

    public void SetPlay();

    public void TurnEnd();

    public void Initial();
}
