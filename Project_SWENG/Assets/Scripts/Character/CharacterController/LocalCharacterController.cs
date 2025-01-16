using System.Collections.Generic;
using UnityEngine;
using EHTool;

public class LocalCharacterController : MonoBehaviour, ICharacterController {

    public int TeamIdx { get; private set; }

    IActionSelector _actionSelector;
    public HexCoordinate HexPos => HexCoordinate.ConvertFromVector3(transform.position);

    [SerializeField] int _dicePoint = 0;

    public CharacterStatus Status { get; private set; }
    CharacterMove _moveComp;
    CharacterAttack _attackComp;

    [SerializeField] bool _rollDice = false;

    public void UsePoint(int usingAmount)
    {
        if (_dicePoint < usingAmount)
        {
            return;
        }
        _dicePoint -= usingAmount;
    }

    public int GetPoint()
    {
        return _dicePoint;
    }

    public virtual void SetPoint(int setValue)
    {
        _dicePoint = setValue;
        _rollDice = true;
        ActionEnd();
    }

    // Start is called before the first frame update
    public void Initial(string characterName, int teamIdx, bool camSync)
    {
        Debug.Log(characterName);

        GameObject go = AssetOpener.ImportGameObject(string.Format("Prefab/Characters/{0}", characterName));

        go.transform.SetParent(transform);
        go.transform.localPosition = Vector3.zero;

        Status = go.GetComponent<CharacterStatus>();
        _moveComp = go.GetComponent<CharacterMove>();
        _attackComp = go.GetComponent<CharacterAttack>();

        if (_moveComp == null)
            _moveComp = gameObject.AddComponent<CharacterMove>();
        if (_attackComp == null)
            _attackComp = gameObject.AddComponent<CharacterAttack>();

        Status.SetCC(this);
        _moveComp.SetCC(this);
        _attackComp.SetCC(this);


        TeamIdx = teamIdx;
        GameManager.Instance.GameMaster.AddTeamMember(this, teamIdx);

        MapUnit mapUnit = HexGrid.Instance.GetMapUnitAt(transform.position);

        mapUnit.SetEntity(gameObject);
        mapUnit.SetCC(this);
    }

    public void CamSetting(string key) {
        CameraManager.Instance.CameraSetting(transform, key);
    }

    public void PlayAnim(string triggerType, string triggerValue) {
        Status.PlayAnim(triggerType, triggerValue);
    }

    public void TakeDamage(int amount)
    {
        Status.TakeDamage(amount);

        if (Status.IsAlive)
        {
            PlayAnim("SetTrigger", "Hit");
            return;
        }

        HexGrid.Instance.GetMapUnitAt(gameObject.transform.position).SetEntity(null);

        PlayAnim("SetTrigger", "Die");
        _actionSelector.Die();

        // 죽었을 때 GameMaster에서 처리할 것
        GameManager.Instance.GameMaster.RemoveTeamMember(this, TeamIdx);

    }

    public void SetPlay() {
        _rollDice = false;
        ActionEnd();
    }

    public void SetActionSelector(IActionSelector actionSelector) {
        _actionSelector = actionSelector;
        _actionSelector.SetCharacterController(this);
    }

    public void ActionEnd(float time = 0) {
        Invoke(nameof(_ActionEnd), time);
    }

    public void _ActionEnd()
    {
        if (_actionSelector == null) {
            return;
        }

        List<IActionSelector.Action> list = new List<IActionSelector.Action>();

        if (_rollDice == false) list.Add(IActionSelector.Action.Dice);
        if (GetPoint() > 0) list.Add(IActionSelector.Action.Interaction);

        _moveComp.TryAddAction(list);
        _attackComp.TryAddAction(list);

        _actionSelector.Ready(list);
    }

    public void TurnEnd() {
        GameManager.Instance.GameMaster.TurnEnd(this);
    }

    public void MoveTo(HexCoordinate before, HexCoordinate after)
    {
        HexGrid.Instance.GetMapUnitAt(before).SetEntity(null);
        HexGrid.Instance.GetMapUnitAt(after).SetEntity(gameObject);
        HexGrid.Instance.GetMapUnitAt(after).SetCC(this);
    }

    public void Move(Queue<Vector3> path)
    {
        _moveComp.Move(path);
    }

    public void EquipItem(string data) { 
        
    }

    public void Interaction(HexCoordinate targetPos)
    {
        HexGrid.Instance.GetMapUnitAt(targetPos).Interaction(this);
    }
}