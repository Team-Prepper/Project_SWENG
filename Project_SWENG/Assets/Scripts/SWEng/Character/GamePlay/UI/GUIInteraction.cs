using System.Collections.Generic;
using UnityEngine;
using SWEng;
using CameraSystem;
using BKTools.Gaming.GridMap2D;

public class GUIInteraction : GUICustomFullScreen {

    [SerializeField] private Sprite _markerSprite;
    [SerializeField] private Vector3 _markerLocalScale;
    [SerializeField] private Vector3 _markerEulerAngle;

    private ISet<GridCoord2D> _interactionRange;
    private int _useMarkCount;

    MapUnit _interactionTarget;
    ICharacter _cc;

    public void Set(ICharacter cc)
    {
        _cc = cc;

        _interactionRange = MapUnitManager.
            Instance.GetNeighboursFor(cc.EntityTransform.Pos);
        _interactionRange.Remove(cc.EntityTransform.Pos);
        
        _interactionTarget = null;

        foreach (GridCoord2D neighbour in _interactionRange)
        {
            _SetMarker(neighbour);
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
        foreach (GridCoord2D coord in _interactionRange)
        {
            MapUnitManager.Instance.GetMapUnitAt(coord).
                SetSprite(_markerSprite, _markerLocalScale, _markerEulerAngle, false);
        }


    }

    public void DoInteraction()
    {
        _cc.Interaction(_interactionTarget.Pos);

        Close();

    }

    public override void HexSelect(GridCoord2D selectGridPos)
    {
        if (_interactionTarget && _interactionTarget == MapUnitManager.Instance.GetMapUnitAt(selectGridPos))
        {
            DoInteraction();
            return;
        }

        _ResetMarker();

        if (_interactionRange.Contains(selectGridPos))
        {
            _interactionTarget = MapUnitManager.Instance.GetMapUnitAt(selectGridPos);
            _SetMarker(selectGridPos);

            return;

        }

        if (_interactionTarget == null)
        {
            _cc.ActionEnd(0);
            Close();
            return;
        }

        _interactionTarget = null;

        foreach (GridCoord2D pos in _interactionRange)
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
