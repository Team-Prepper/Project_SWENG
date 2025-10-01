using System.Collections.Generic;
using UnityEngine;
using SWEng;
using EasyH;
using BKTools.Gaming.GridMap2D;

public class GUIMoving : GUICustomFullScreen {

    [SerializeField] private Sprite _markerSprite;
    [SerializeField] private Vector3 _markerLocalScale;
    [SerializeField] private Vector3 _markerEulerAngle;


    [SerializeField] private Transform[] _moveNumPrefabs;
    [SerializeField] private Transform _moveNumParent;

    [SerializeField] private ICharacter _cc;
    private GridCoord2D? _selectedPos;

    private ISearchResult<GridCoord2D> _movementRange
        = new SearchResult<GridCoord2D>();
    private IList<GridCoord2D> _currentPath = new List<GridCoord2D>();

    public void Set(ICharacter target)
    {
        _selectedPos = null;

        _cc = target;

        _CalcualteRange();
        _ShowRange();
        _moveNumParent.localScale = Vector3.one / GameObject.Find("Canvas").GetComponent<RectTransform>().localScale.y;

        _cc.CameraController.CamSetting("Wide");
    }
    private void _HideRange()
    {
        if (_movementRange.GetAllState() == null) return;

        foreach (GridCoord2D hexPosition in
            _movementRange.GetAllState())
        {
            MapUnitManager.Instance.GetMapUnitAt(hexPosition).
                SetSprite(_markerSprite, _markerLocalScale, _markerEulerAngle, false);
        }

        _HideMoveNum();
    }

    private void _ShowRange()
    {
        GridCoord2D unitPos = _cc.EntityTransform.Pos;

        foreach (GridCoord2D hexPosition in _movementRange.GetAllState())
        {
            if (unitPos.Equals(hexPosition))
                continue;

            MapUnitManager.Instance.GetMapUnitAt(hexPosition).
                SetSprite(_markerSprite, _markerLocalScale, _markerEulerAngle, true);
        }
    }

    private void _ShowPath(GridCoord2D selectedHexPosition)
    {

        _HideRange();
        
        _currentPath = _movementRange.GetPathToState(
            selectedHexPosition);
        _currentPath.RemoveAt(0);
        
        _moveNumParent.gameObject.SetActive(true);

        int i = 0;

        foreach (GridCoord2D hexPosition in _currentPath)
        {
            MapUnit pathHex = MapUnitManager.Instance.GetMapUnitAt(hexPosition);
            _moveNumPrefabs[Mathf.Clamp(i += pathHex.Cost, 0, 9)].position = pathHex.transform.position;
        }
    }

    private void _HideMoveNum()
    {
        _moveNumParent.gameObject.SetActive(false);
        for (int i = 0; i < _moveNumPrefabs.Length; i++)
        {
            _moveNumPrefabs[i].transform.localPosition = Vector3.zero;
        }
    }

    private void _CalcualteRange()
    {
        _movementRange = EntityManager.Instance.
            GetPathGroup(_cc.EntityTransform.Pos, _cc.DicePoint.GetPoint());
    }

    private void _MoveUnit()
    {       
        _cc.Move(_currentPath);
        _HideRange();
        Close();
    }

    public override void HexSelect(GridCoord2D selectGridPos)
    {

        if (_selectedPos != null &&
            selectGridPos.Equals(_selectedPos))
        {
            _MoveUnit();

            return;
        }

        if (_IsHexCoordInPathGroup(selectGridPos))
        {
            _ShowPath(selectGridPos);
            _selectedPos = selectGridPos;
            
            return;
        }

        if (_selectedPos != null) {
            _selectedPos = null;
            _ShowPath(_cc.EntityTransform.Pos);
            _ShowRange();
            return;
        }

        _cc.ActionEnd(0);
        Close();

    }

    private bool _IsHexCoordInPathGroup(GridCoord2D pos) {
        if (pos.Equals(_cc.EntityTransform.Pos)) return false;
        return _movementRange.ContainsState(pos);
    }

    public override void Close()
    {
        _HideRange();
        _ShowPath(_cc.EntityTransform.Pos);
        base.Close();
    }
}
