using System.Collections.Generic;
using UnityEngine;

public class GUIInteraction : GUICustomFullScreen {

    [SerializeField] private Sprite _markerSprite;
    [SerializeField] private Vector3 _markerLocalScale;
    [SerializeField] private Vector3 _markerEulerAngle;

    private ISet<HexCoordinate> _interactionRange;
    private int _useMarkCount;

    MapUnit _interactionTarget;
    ICharacterController _cc;

    public void Set(ICharacterController cc)
    {
        _cc = cc;

        _interactionRange = HexGrid.Instance.GetNeighboursFor(cc.HexPos);
        _interactionTarget = null;

        foreach (HexCoordinate neighbour in _interactionRange)
        {
            _SetMarker(neighbour);
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
        foreach (HexCoordinate coord in _interactionRange)
        {
            HexGrid.Instance.GetMapUnitAt(coord).
                SetSprite(_markerSprite, _markerLocalScale, _markerEulerAngle, false);
        }


    }

    public void DoInteraction()
    {
        _cc.Interaction(_interactionTarget.HexCoords);
        _cc.UsePoint(1);

        Close();

    }

    public override void HexSelect(HexCoordinate selectGridPos)
    {
        if (_interactionTarget && _interactionTarget == HexGrid.Instance.GetMapUnitAt(selectGridPos))
        {
            DoInteraction();
            return;
        }

        _ResetMarker();

        if (_interactionRange.Contains(selectGridPos))
        {
            _interactionTarget = HexGrid.Instance.GetMapUnitAt(selectGridPos);
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

        foreach (HexCoordinate pos in _interactionRange)
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
