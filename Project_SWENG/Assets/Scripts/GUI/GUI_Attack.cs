using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Character;
using UISystem;

public class GUI_Attack : GUIFullScreen {

    public Character.NetworkCharacterController _target;

    [SerializeField] private Transform _markerParent;
    [SerializeField] private Transform[] _attackMarkers;

    [SerializeField] private List<HexCoordinate> _attackRange;

    // Start is called before the first frame update
    protected override void Open(Vector2 openPos)
    {
        base.Open(openPos);
    }

    public void Set(GameObject target) {

        _target = target.GetComponent<NetworkCharacterController>();

        HexCoordinate curHexPos = HexCoordinate.ConvertFromVector3(target.transform.position);
        _markerParent.localScale = Vector3.one / GameObject.Find("Canvas").GetComponent<RectTransform>().localScale.y;

        int i = 0;
        _attackRange = new List<HexCoordinate>();

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

    public override void HexSelect(HexCoordinate selectGridPos)
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
