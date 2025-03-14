using System.Collections.Generic;
using UnityEngine;

public class GUI_AttackSelect : GUICustomFullScreen, ISkillTargetSelector {

    [SerializeField] private Sprite _markerSprite;
    [SerializeField] private Vector3 _markerLocalScale;
    [SerializeField] private Vector3 _markerEulerAngle;

    [SerializeField] private GameObject btnAttack;
    [SerializeField] private GameObject btnSkill;

    private ISet<HexCoordinate> _attackRange;

    private MapUnit _attackTarget;

    private ISkill _targetAttack;
    private ICharacterController _cc;

    public void Set(ISkill attack, ICharacterController cc)
    {
        _targetAttack = attack;
        _cc = cc;

        _attackRange = new HashSet<HexCoordinate>(6);

        _attackTarget = null;

        foreach (var neighbour in HexGrid.Instance.GetNeighboursFor(cc.HexPos))
        {
            MapUnit atkHex = HexGrid.Instance.GetMapUnitAt(neighbour);

            if (!(atkHex.tileType == TileDataScript.TileType.normal ||
                atkHex.tileType == TileDataScript.TileType.dungon)) continue;

            _attackRange.Add(neighbour);
            _SetMarker(atkHex.HexCoords);
        }

        CameraManager.Instance.CameraSetting(_cc.transform, "Wide");
    }

    private void _SetMarker(HexCoordinate pos)
    {
        HexGrid.Instance.GetMapUnitAt(pos).
            SetSprite(_markerSprite, _markerLocalScale, _markerEulerAngle, true);

    }

    private void _ResetMarker()
    {
        foreach(HexCoordinate coord in _attackRange) {
            HexGrid.Instance.GetMapUnitAt(coord).
                SetSprite(_markerSprite, _markerLocalScale, _markerEulerAngle, false);
        }

    }

    public void DoAttack()
    {
        IList<IDamagable> target = new List<IDamagable>() { _attackTarget.CC };
        _targetAttack.Attack(target, _attackTarget.HexCoords.ConvertToVector3() );

        Close();

    }

    public override void HexSelect(HexCoordinate selectGridPos)
    {

        if (_attackTarget && _attackTarget == HexGrid.Instance.GetMapUnitAt(selectGridPos))
        {
            DoAttack();
            return;
        }

        _ResetMarker();

        if (_attackRange.Contains(selectGridPos))
        {
            _attackTarget = HexGrid.Instance.GetMapUnitAt(selectGridPos);
            _SetMarker(selectGridPos);

            return;

        }

        if (_attackTarget == null) {
            _cc.ActionEnd(0);
            Close();
            return;
        }

        _attackTarget = null;

        foreach (HexCoordinate pos in _attackRange)
        {
            _SetMarker(pos);
        }
    }

    public override void Close()
    {
        base.Close();
        _ResetMarker();
    }
}
