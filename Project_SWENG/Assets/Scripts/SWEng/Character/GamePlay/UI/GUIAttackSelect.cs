using System.Collections.Generic;
using System;
using UnityEngine;
using SWEng;
using CameraSystem;
using BKTools.Gaming.GridMap2D;

public class GUIAttackSelect : GUICustomFullScreen, ISkillTargetSelector
{

    [SerializeField] private Sprite _markerSprite;
    [SerializeField] private Vector3 _markerLocalScale;
    [SerializeField] private Vector3 _markerEulerAngle;

    [SerializeField] private GameObject btnAttack;
    [SerializeField] private GameObject btnSkill;

    private ISet<GridCoord2D> _attackRange;

    private MapUnit _attackTarget;

    private Action<IList<IDamageable>> _targetAttack;
    private ICharacter _cc;

    public void Set(Action<IList<IDamageable>> attack, ICharacter cc)
    {
        _targetAttack = attack;
        _cc = cc;

        _attackRange = new HashSet<GridCoord2D>(6);

        _attackTarget = null;

        foreach (var neighbour in MapUnitManager.
            Instance.GetNeighboursFor(cc.EntityTransform.Pos))
        {
            if (neighbour.Equals(cc.EntityTransform.Pos)) continue;
            
            MapUnit atkHex = MapUnitManager.
                Instance.GetMapUnitAt(neighbour);

            if (!(atkHex.tileType == TileDataScript.TileType.normal ||
                atkHex.tileType == TileDataScript.TileType.dungon)) continue;

            _attackRange.Add(neighbour);
            _SetMarker(atkHex.Pos);
        }

        CameraManager.Instance.CameraSetting(_cc.transform, "Wide");
    }

    private void _SetMarker(GridCoord2D pos)
    {
        MapUnitManager.Instance.GetMapUnitAt(pos).
            SetSprite(_markerSprite, _markerLocalScale, _markerEulerAngle, true);

    }

    private void _ResetMarker()
    {
        foreach (GridCoord2D coord in _attackRange)
        {
            MapUnitManager.Instance.GetMapUnitAt(coord).
                SetSprite(_markerSprite, _markerLocalScale, _markerEulerAngle, false);
        }

    }

    public void DoAttack()
    {
        IList<IDamageable> target = new List<IDamageable>() {
            DamageableManager.Instance.
                GetDamageableAt(_attackTarget.Pos) };
                
        _targetAttack?.Invoke(target);

        Close();
        
    }

    public override void HexSelect(GridCoord2D selectGridPos)
    {

        if (_attackTarget && _attackTarget == MapUnitManager.Instance.GetMapUnitAt(selectGridPos))
        {
            DoAttack();
            return;
        }

        _ResetMarker();

        if (_attackRange.Contains(selectGridPos))
        {
            _attackTarget = MapUnitManager.Instance.GetMapUnitAt(selectGridPos);
            _SetMarker(selectGridPos);

            return;

        }

        if (_attackTarget == null)
        {
            _cc.ActionEnd(0);
            Close();
            return;
        }

        _attackTarget = null;

        foreach (GridCoord2D pos in _attackRange)
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