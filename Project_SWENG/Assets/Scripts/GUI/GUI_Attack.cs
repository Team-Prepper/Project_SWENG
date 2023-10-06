using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Character;
using static UnityEditor.PlayerSettings;

public class GUI_Attack : GUIFullScreen {

    public Character.CharacterController _target;

    [SerializeField] private Transform[] _attackMarkers;

    [SerializeField] private List<Vector3Int> _attackRange;

    // Start is called before the first frame update
    protected override void Open(Vector2 openPos)
    {
        base.Open(openPos);
    }

    public void Set(GameObject target) {

        _target = target.GetComponent<Character.CharacterController>();

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
}
