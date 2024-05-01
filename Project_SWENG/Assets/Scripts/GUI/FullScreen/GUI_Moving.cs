using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UISystem;
using CharacterSystem;

public class GUI_Moving : GUIFullScreen {

    [SerializeField] private PlayerCharacter _target;
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

        HexCoordinate unitPos = HexCoordinate.ConvertFromVector3(_target.transform.position);

        foreach (HexCoordinate hexPosition in movementRange.GetRangePositions())
        {
            if (unitPos.Equals(hexPosition))
                continue;

            //Debug.Log(hexPosition);
            HexGrid.Instance.GetTileAt(hexPosition).EnableHighlight();
        }
    }

    public void _ShowPath(HexCoordinate selectedHexPosition)
    {

        _HideRange();

        //Debug.Log("Target: " + selectedHexPosition);
        
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
        movementRange = GraphSearch.BFSGetRange(HexCoordinate.ConvertFromVector3(_target.transform.position), _target.GetPoint());
    }

    private void _MoveUnit()
    {
        Queue<Vector3> path = new Queue<Vector3>(currentPath.Select(pos => HexGrid.Instance.GetTileAt(pos).transform.position).ToList());
        _target.Move(path);
        _HideRange();
        Close();
    }

    // Start is called before the first frame update
    protected override void Open(Vector2 openPos)
    {
        base.Open(openPos);
        CamMovement.Instance.ConvertWideCamera();
    }

    public void Set(PlayerCharacter target)
    {
        CamMovement.Instance.IsPlayerMove = true;
        CamMovement.Instance.SetCamTarget(target.gameObject);

        _target = target;

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

            CamMovement.Instance.ConvertCharacterCam();
            return;
        }

        if (!movementRange.IsHexPositionInRange(selectGridPos) ||
            selectGridPos == HexCoordinate.ConvertFromVector3(_target.transform.position))
        {
            _HideRange();

            CamMovement.Instance.ConvertCharacterCam();
            Close();
            return;
        }


        _ShowPath(selectGridPos);
        _selectedPos = selectGridPos;

    }
}
