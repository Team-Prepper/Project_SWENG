using CharacterSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemyActionSelector : MonoBehaviour, IActionSelector {
    
    ICharacterController _targetCC;

    public void SetCharacterController(ICharacterController cc)
    {
        _targetCC = cc;

    }

    public void Ready(IList<Character.Action> actionList)
    {
        if (actionList.Contains(Character.Action.Attack)) {
           _targetCC.UseAttack(0);
            return;
        }
        _targetCC.TurnEnd();
    }
}
