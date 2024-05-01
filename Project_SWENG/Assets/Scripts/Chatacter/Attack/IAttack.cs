using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAttack
{
    public void Attack();
    public int GetAttackValue();
}

public class BasicAttack : IAttack {

    ICharacterController _cc;
    Vector3 _pos;

    int _value;

    public BasicAttack(ICharacterController cc,Vector3 pos, int value) {
        _cc = cc;
        _pos = pos;
        _value = value;
    }

    public void Attack() {
        List<HexCoordinate> attackPos = new List<HexCoordinate>();
        attackPos.AddRange(HexGrid.Instance.GetNeighboursFor(HexCoordinate.ConvertFromVector3(_pos)));
        _cc.Attack(attackPos, _value);
    }

    public int GetAttackValue() {
        return _value;
    }
}