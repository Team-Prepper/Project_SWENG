using System.Collections;
using System.Collections.Generic;
using EHTool.UIKit;
using UnityEngine;
using CharacterSystem;

public interface IAttack
{
    public void Attack(IList<HexCoordinate> attackPos);
}

public interface IAttackTargetSelector {
    public void Set(IAttack attack, Vector3 pos);
}

public class RangeTargetSelector : IAttackTargetSelector {
    public void Set(IAttack attack, Vector3 pos)
    {
        List<HexCoordinate> attackPos = new List<HexCoordinate>();
        attackPos.AddRange(HexGrid.Instance.GetNeighboursFor(HexCoordinate.ConvertFromVector3(pos)));
        attack.Attack(attackPos);
    }

}

public class BasicTargetingAttack : IAttack {

    ICharacterController _cc;
    Character _c;
    int _usingPoint;

    int _value;

    public BasicTargetingAttack(ICharacterController cc, Character c, Vector3 pos, int value, int point)
    {
        _cc = cc;
        _c = c;

        _usingPoint = point;
        _value = value;

        UIManager.Instance.OpenGUI<GUI_AttackSelect>("AttackSelect").Set(this, pos);

    }

    public void Attack(IList<HexCoordinate> attackPos)
    {
        _cc.Attack(attackPos, _value * _usingPoint, 1.2f);
        _c.UsePoint(_usingPoint);

    }
}

public class BasicAttack : IAttack {

    ICharacterController _cc;

    int _value;

    public BasicAttack(ICharacterController cc,Vector3 pos, int value) {
        _cc = cc;
        _value = value;

        new RangeTargetSelector().Set(this, pos);
    }


    public void Attack(IList<HexCoordinate> attackPos) {
        _cc.Attack(attackPos, _value, 3f);
    }

}