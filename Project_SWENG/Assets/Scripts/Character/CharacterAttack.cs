using UnityEngine;
using System.Collections.Generic;

public class CharacterAttack : MonoBehaviour, ICharacterComponent {

    ICharacterController _cc;
    [SerializeField] int _usePointAtAttack = 3;

    public void SetCC(ICharacterController cc)
    {
        _cc = cc;
    }

    public void TryAddAction(IList<CharacterStatus.Action> target)
    {
        if (_cc.GetPoint() < _usePointAtAttack) return;

        target.Add(CharacterStatus.Action.Attack);
    }

}