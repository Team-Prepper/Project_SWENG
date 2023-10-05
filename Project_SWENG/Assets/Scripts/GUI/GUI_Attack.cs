using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class GUI_Attack : GUIFullScreen {

    public Character _target;

    [SerializeField] private Transform[] _attackMarkers;

    [SerializeField] private List<Vector3Int> _attackRange;

    // Start is called before the first frame update
    protected override void Open(Vector2 openPos)
    {
        base.Open(openPos);
    }

    public void Set(GameObject target) {

        _target = target.GetComponent<Character>();

        Vector3Int curHexPos = HexGrid.Instance.GetClosestHex(target.transform.position);

        int i = 0;
        _attackRange = new List<Vector3Int>();

        foreach (var neighbour in HexGrid.Instance.GetNeighboursFor(curHexPos))
        {
            Hex atkHex = HexGrid.Instance.GetTileAt(neighbour);

            if (atkHex.tileType != Hex.Type.Field) continue;

            _attackRange.Add(neighbour);
            _attackMarkers[i++].position = atkHex.transform.position;
        }

        CamMovement.Instance.ConvertMovementCamera();
        CamMovement.Instance.CamSetToPlayer(target);

    }

    public override void HexSelect(Vector3Int selectGridPos)
    {
        Debug.Log("Try Attack At" + selectGridPos);

        if (_attackRange.Contains(selectGridPos))
        {
            AttackManager.Instance.BaseAtkHandler(_target, HexGrid.Instance.GetTileAt(selectGridPos));
            Debug.Log("Success");
        }
        Close();
    }


    public Vector3Int MousePointHex()
    {
        Vector3 touchPos = PlayerInputManager.Instance.mousePos;
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(touchPos);

        if (!Physics.Raycast(ray, out hit, 100, selectionMask)) return Vector3Int.zero;

        return HexGrid.Instance.GetClosestHex(hit.collider.gameObject.transform.position);

    }
}
