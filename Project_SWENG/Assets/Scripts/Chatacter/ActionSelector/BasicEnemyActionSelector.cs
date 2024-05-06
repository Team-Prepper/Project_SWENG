using CharacterSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemyActionSelector : IActionSelector {
    
    ICharacterController _targetCC;
    EnemyPlayer _player;

    public BasicEnemyActionSelector(EnemyPlayer player) {
        _player = player;
        _player.AddSelector(this);
    }

    public void SetCharacterController(ICharacterController cc)
    {
        _targetCC = cc;

    }

    public void Ready(IList<Character.Action> actionList)
    {
        _player.ActionAdd(this, actionList);
    }
    public void Die()
    {
        _player.RemoveSelector(this);
    }

    public void CamSetting() {
        _targetCC.CamSetting();
    }

    public void DoAction(Character.Action action)
    {

        switch (action) {
            case Character.Action.Attack:
                _targetCC.DoAttack();
                return;
            case Character.Action.Move:
                _targetCC.DoMove();
                return;
            default:
                _targetCC.TurnEnd();
                return;

        }

    }

}
