using System.Collections.Generic;
using UnityEngine;

public class GUI_AttackSelect : GUICustomFullScreen, IAttackTargetSelector {

    [SerializeField] private Transform _markerParent;
    [SerializeField] private Transform[] _attackMarkers;

    [SerializeField] private GameObject btnAttack;
    [SerializeField] private GameObject btnSkill;

    private IList<HexCoordinate> _attackRange;
    private int _useMarkCount;

    MapUnit _attackTarget;
    IAttack _targetAttack;
    ICharacterController _cc;


    public void Set(IAttack attack, ICharacterController cc)
    {
        _targetAttack = attack;
        _cc = cc;

        _markerParent.localScale = Vector3.one / GameObject.Find("Canvas").GetComponent<RectTransform>().localScale.y;
        _attackRange = new List<HexCoordinate>();

        _attackTarget = null;

        foreach (var neighbour in HexGrid.Instance.GetNeighboursFor(cc.HexPos))
        {
            MapUnit atkHex = HexGrid.Instance.GetTileAt(neighbour);

            if (!(atkHex.tileType == TileDataScript.TileType.normal || atkHex.tileType == TileDataScript.TileType.dungon)) continue;

            _attackRange.Add(neighbour);
            _SetMarker(atkHex.transform.position);
        }

        CameraManager.Instance.ConverTo(_cc.transform, "Wide");
    }

    private void _SetMarker(Vector3 pos)
    {
        if (_useMarkCount >= _attackMarkers.Length) return;

        _attackMarkers[_useMarkCount].gameObject.SetActive(true);
        _attackMarkers[_useMarkCount++].position = pos;

    }

    private void _ResetMarker()
    {
        for (int i = 0; i < _useMarkCount; i++)
        {
            _attackMarkers[i].gameObject.SetActive(false);
        }

        _useMarkCount = 0;

    }

    public void DoAttack()
    {
        IList<HexCoordinate> target = new List<HexCoordinate>() { _attackTarget.HexCoords };
        _targetAttack.Attack(target);

        Close();

    }

    public override void HexSelect(HexCoordinate selectGridPos)
    {

        if (_attackTarget && _attackTarget == HexGrid.Instance.GetTileAt(selectGridPos))
        {
            DoAttack();
        }

        _ResetMarker();

        if (_attackRange.Contains(selectGridPos))
        {
            _attackTarget = HexGrid.Instance.GetTileAt(selectGridPos);
            _SetMarker(_attackTarget.transform.position);

            return;

        }

        if (_attackTarget == null) {
            _cc.ActionEnd();
            Close();
            return;
        }

        _attackTarget = null;

        foreach (HexCoordinate pos in _attackRange)
        {
            _SetMarker(pos.ConvertToVector3());
        }
    }
}
