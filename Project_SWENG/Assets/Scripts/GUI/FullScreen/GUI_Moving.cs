using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UISystem;

public class GUI_Moving : GUIFullScreen {

    private DicePoint _targetPoint;
    [SerializeField] private NetworkUnit _targetUnit;
    private HexCoordinate _selectedPos;

    [SerializeField] private Transform[] _moveNumPrefabs;
    [SerializeField] private Transform _moveNumParent;

    private BFSResult movementRange = new BFSResult();
    private List<HexCoordinate> currentPath = new List<HexCoordinate>();

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

        HexCoordinate unitPos = HexCoordinate.ConvertFromVector3(_targetPoint.transform.position);

        foreach (HexCoordinate hexPosition in movementRange.GetRangePositions())
        {
            if (unitPos.Equals(hexPosition))
                continue;

            Debug.Log(hexPosition);
            HexGrid.Instance.GetTileAt(hexPosition).EnableHighlight();
        }
    }

    public void _ShowPath(HexCoordinate selectedHexPosition)
    {

        _HideRange();

        Debug.Log("Target: " + selectedHexPosition);
        
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
        movementRange = GraphSearch.BFSGetRange(HexCoordinate.ConvertFromVector3(_targetPoint.transform.position), _targetPoint.GetPoint());
    }

    private void _MoveUnit()
    {
        Debug.Log("Moving unit " + _targetUnit.name);
        _targetUnit.NewMoveThroughPath(currentPath.Select(pos => HexGrid.Instance.GetTileAt(pos).transform.position).ToList());
        _HideRange();
        Close();
    }

    // Start is called before the first frame update
    protected override void Open(Vector2 openPos)
    {
        base.Open(openPos);
        CamMovement.Instance.ConvertMovementCamera();
    }

    public void Set(GameObject target)
    {
        CamMovement.Instance.IsPlayerMove = true;
        _targetPoint = target.GetComponent<DicePoint>();
        _targetUnit = target.GetComponent<NetworkUnit>();
        CamMovement.Instance.CamSetToPlayer(target);

        _CalcualteRange();
        _ShowRange();
        _moveNumParent.localScale = Vector3.one / GameObject.Find("Canvas").GetComponent<RectTransform>().localScale.y;
    }

    public override void HexSelect(HexCoordinate selectGridPos)
    {
        CamMovement.Instance.IsPlayerMove = false;

        if (_selectedPos != null && selectGridPos == _selectedPos)
        {
            _MoveUnit();
            return;
        }

        if (!movementRange.IsHexPositionInRange(selectGridPos) ||
            selectGridPos == HexCoordinate.ConvertFromVector3(_targetPoint.transform.position))
        {
            _HideRange();
            Close();
            return;
        }


        _ShowPath(selectGridPos);
        _selectedPos = selectGridPos;

    }
}
