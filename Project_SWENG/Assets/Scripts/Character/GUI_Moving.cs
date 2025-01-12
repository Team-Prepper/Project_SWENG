using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GUI_Moving : GUICustomFullScreen {

    [SerializeField] private Sprite _glowSprite;

    [SerializeField] private Transform[] _moveNumPrefabs;
    [SerializeField] private Transform _moveNumParent;

    [SerializeField] private ICharacterController _cc;
    private HexCoordinate? _selectedPos;

    private IPathGroup movementRange = new BFSResult();
    private IList<HexCoordinate> currentPath = new List<HexCoordinate>();

    private void _HideRange()
    {
        if (movementRange.GetRangePositions() == null) return;

        foreach (HexCoordinate hexPosition in movementRange.GetRangePositions())
        {
            HexGrid.Instance.GetTileAt(hexPosition).SetSprite(_glowSprite, false);
        }

        _HideMoveNum();
    }

    private void _ShowRange()
    {

        HexCoordinate unitPos = _cc.HexPos;

        foreach (HexCoordinate hexPosition in movementRange.GetRangePositions())
        {
            if (unitPos.Equals(hexPosition))
                continue;

            HexGrid.Instance.GetTileAt(hexPosition).SetSprite(_glowSprite, true);
        }
    }

    private void _ShowPath(HexCoordinate selectedHexPosition)
    {

        _HideRange();
        
        currentPath = movementRange.GetPathTo(selectedHexPosition);
        _moveNumParent.gameObject.SetActive(true);

        int i = 0;
        foreach (HexCoordinate hexPosition in currentPath)
        {
            MapUnit pathHex = HexGrid.Instance.GetTileAt(hexPosition);
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
        movementRange = HexGrid.Instance.GetPathGroup(_cc.HexPos, _cc.GetPoint());
    }

    private void _MoveUnit()
    {
        Queue<Vector3> path = new Queue<Vector3>(currentPath.Select(pos => HexGrid.Instance.GetTileAt(pos).transform.position).ToList());
        _cc.Move(path);
        _HideRange();
        Close();
    }

    public void Set(ICharacterController target)
    {
        _selectedPos = null;

        _cc = target;

        _CalcualteRange();
        _ShowRange();
        _moveNumParent.localScale = Vector3.one / GameObject.Find("Canvas").GetComponent<RectTransform>().localScale.y;

        _cc.CamSetting("Wide");
    }

    public override void HexSelect(HexCoordinate selectGridPos)
    {

        if (_selectedPos != null && selectGridPos == _selectedPos || !movementRange.IsHexCroodInRange(selectGridPos))
        {
            _MoveUnit();

            return;
        }

        if (selectGridPos.Equals(_cc.HexPos)) {
            _cc.ActionEnd();
            Close();
        }

        _ShowPath(selectGridPos);
        _selectedPos = selectGridPos;

    }
}
