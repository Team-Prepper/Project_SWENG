using System.Collections.Generic;
using System.Linq;
using EHTool.UIKit;
using UnityEngine;

public interface IAttack
{
    public void Attack(IList<HexCoordinate> attackPos);
}

public interface IAttackTargetSelector {
    public void Set(IAttack attack, ICharacterController cc);
}

public class RangeTargetSelector : IAttackTargetSelector {
    public void Set(IAttack attack, ICharacterController cc)
    {
        List<HexCoordinate> attackPos = new List<HexCoordinate>();
        attackPos.AddRange(HexGrid.Instance.GetNeighboursFor(cc.HexPos));
        attack.Attack(attackPos);
    }

}

public class BasicTargetingAttack : IAttack {

    ICharacterController _cc;
    int _usingPoint;

    int _value;

    public BasicTargetingAttack(ICharacterController cc, int value, int point)
    {
        _cc = cc;

        _usingPoint = point;
        _value = value;

        UIManager.Instance.OpenGUI<GUI_AttackSelect>
            ("AttackSelect").Set(this, _cc);

    }

    public void Attack(IList<HexCoordinate> attackPos)
    {
        foreach (HexCoordinate hexPos in attackPos)
        {
            MapUnit targetHex = HexGrid.Instance.GetMapUnitAt(hexPos);
            if (targetHex.Entity == null || !targetHex.Entity.TryGetComponent(out IDamagable target)) continue;

            target.TakeDamage(_value * _usingPoint);

        }

        if (attackPos.Count > 0)
            _cc.transform.LookAt(attackPos.ElementAt(0).ConvertToVector3() + _cc.transform.position.y * Vector3.up);

        _cc.UsePoint(_usingPoint);
        _cc.CamSetting("Battle");
        _cc.PlayAnim("SetTrigger", "Attack");
        _cc.ActionEnd(2f);

    }
}

public class BasicAttack : IAttack {

    ICharacterController _cc;

    int _value;

    public BasicAttack(ICharacterController cc, int value) {
        _cc = cc;
        _value = value;

        new RangeTargetSelector().Set(this, _cc);
    }


    public void Attack(IList<HexCoordinate> attackPos)
    {
        HexCoordinate? attackedCoord = null;

        foreach (HexCoordinate hexPos in attackPos)
        {
            MapUnit targetHex = HexGrid.Instance.GetMapUnitAt(hexPos);
            if (targetHex.Entity == null || !targetHex.Entity.TryGetComponent(out IDamagable target)) continue;

            if (attackedCoord == null) attackedCoord = hexPos;

            target.TakeDamage(_value);

        }

        if (attackedCoord != null)
            _cc.transform.LookAt(attackedCoord.Value.ConvertToVector3() + _cc.transform.position.y * Vector3.up);

        _cc.UsePoint(_value);
        _cc.CamSetting("Wide");
        _cc.PlayAnim("SetTrigger", "Attack");
        _cc.ActionEnd(2f);
    }

}