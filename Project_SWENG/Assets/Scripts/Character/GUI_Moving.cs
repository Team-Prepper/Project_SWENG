using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using CharacterSystem;

public class GUI_Moving : GUICustomFullScreen {

    [SerializeField] private Transform[] _moveNumPrefabs;
    [SerializeField] private Transform _moveNumParent;

    [SerializeField] private Character _target;
    private HexCoordinate? _selectedPos;

    private IPathGroup movementRange = new BFSResult();
    private IList<HexCoordinate> currentPath = new List<HexCoordinate>();

    private void _HideRange()
    {
        if (movementRange.GetRangePositions() == null) return;

        foreach (HexCoordinate hexPosition in movementRange.GetRangePositions())
        {
            HexGrid.Instance.GetTileAt(hexPosition).DisableHighlight();
        }
        _HideMoveNum();
    }

    private void _ShowRange()
    {

        HexCoordinate unitPos = HexCoordinate.ConvertFromVector3(_target.transform.position);

        foreach (HexCoordinate hexPosition in movementRange.GetRangePositions())
        {
            if (unitPos.Equals(hexPosition))
                continue;

            HexGrid.Instance.GetTileAt(hexPosition).EnableHighlight();
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
            Hex pathHex = HexGrid.Instance.GetTileAt(hexPosition);
            pathHex.HighlightPath();
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
        movementRange = HexGrid.Instance.GetPathGroup(HexCoordinate.ConvertFromVector3(_target.transform.position), _target.GetPoint());
    }

    private void _MoveUnit()
    {
        Queue<Vector3> path = new Queue<Vector3>(currentPath.Select(pos => HexGrid.Instance.GetTileAt(pos).transform.position).ToList());
        _target.Move(path);
        _HideRange();
        Close();
    }

    public void Set(Character target)
    {
        _selectedPos = null;

        _target = target;

        _CalcualteRange();
        _ShowRange();
        _moveNumParent.localScale = Vector3.one / GameObject.Find("Canvas").GetComponent<RectTransform>().localScale.y;

        CamMovement.Instance.ConvertToWideCam();
    }

    public override void HexSelect(HexCoordinate selectGridPos)
    {

        if (_selectedPos != null && selectGridPos == _selectedPos || !movementRange.IsHexCroodInRange(selectGridPos))
        {
            _MoveUnit();

            return;
        }


        _ShowPath(selectGridPos);
        _selectedPos = selectGridPos;

    }
}
