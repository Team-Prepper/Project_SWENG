using System.Collections.Generic;
using UnityEngine;
using Character;
using UISystem;

public class GUI_Attack : GUIFullScreen {

    public Character.NetworkCharacterController _target;

    [SerializeField] private Transform _markerParent;
    [SerializeField] private Transform[] _attackMarkers;
    private int _useMarkCount;
    
    [SerializeField] private List<HexCoordinate> _attackRange;

    Hex _attackTarget;

    private int _skillDmg = 0;
    [SerializeField] private GameObject btnAttack;
    [SerializeField] private GameObject btnSkill;

    public void Set(GameObject target, int skillDmg = 0) {
    
        _target = target.GetComponent<NetworkCharacterController>();
        _skillDmg = skillDmg;
        HexCoordinate curHexPos = HexCoordinate.ConvertFromVector3(target.transform.position);
        _markerParent.localScale = Vector3.one / GameObject.Find("Canvas").GetComponent<RectTransform>().localScale.y;

        _attackRange = new List<HexCoordinate>();

        foreach (var neighbour in HexGrid.Instance.GetNeighboursFor(curHexPos))
        {
            Hex atkHex = HexGrid.Instance.GetTileAt(neighbour);

            if (!(atkHex.tileType == TileDataScript.TileType.normal || atkHex.tileType == TileDataScript.TileType.dungon)) continue;

            _attackRange.Add(neighbour);
            _SetMarker(atkHex.transform.position);
        }

        CamMovement.Instance.ConvertMovementCamera();
        CamMovement.Instance.CamSetToPlayer(target);
        
        btnAttack.SetActive(skillDmg == 0);
        btnSkill.SetActive(skillDmg != 0); // (skillDmg > 0)
    }

    private void _SetMarker(Vector3 pos)
    {
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

        if (_attackTarget == null) {
            return;
        }

        AttackManager.Instance.BaseAtkHandler(_target, _attackTarget);
        Close();

    }
    
    public void UseSkill()
    {
        if (_attackTarget == null) {
            return;
        }
        
        AttackManager.Instance.SkillAtkHandler(_target, _attackTarget, _skillDmg);
        Close();
    }

    public override void HexSelect(HexCoordinate selectGridPos)
    {

        if (_attackTarget && _attackTarget == HexGrid.Instance.GetTileAt(selectGridPos)) {
            if(_skillDmg != 0)
            {
                UseSkill();
            }
            else
            {
                DoAttack();
            }
            
            return;
        }

        _ResetMarker();

        if (!_attackRange.Contains(selectGridPos))
        {
            _attackTarget = null;
            foreach (HexCoordinate pos in _attackRange) {
                _SetMarker(pos.ConvertToVector3());
            }
            return;

        }

        _attackTarget = HexGrid.Instance.GetTileAt(selectGridPos);
        _SetMarker(_attackTarget.transform.position);
    }

    public override void OpenWindow(string key)
    {
        if (_nowPopUp) return;
        base.OpenWindow(key);
    }
}
