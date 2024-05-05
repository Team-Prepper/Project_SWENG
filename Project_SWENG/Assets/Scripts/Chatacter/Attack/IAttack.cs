using System.Collections;
using System.Collections.Generic;
using UISystem;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

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
    Vector3 _pos;

    int _value;

    public BasicTargetingAttack(ICharacterController cc, Vector3 pos, int value)
    {
        _cc = cc;
        _pos = pos;
        _value = value;

        UIManager.OpenGUI<GUI_AttackSelect>("AttackSelect").Set(this, pos);

    }

    public void Attack(IList<HexCoordinate> attackPos)
    {
        _cc.Attack(attackPos, _value);

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
        _cc.Attack(attackPos, _value);
    }

}