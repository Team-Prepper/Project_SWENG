using System.Collections.Generic;
using UnityEngine;

public interface ISkill
{
    public void Set(IActionSelector selector, ICharacterController cc);
    public void Attack(IList<IDamagable> attackPos, Vector3 look);

}