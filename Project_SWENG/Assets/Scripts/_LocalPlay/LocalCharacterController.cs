using System.Collections.Generic;
using UnityEngine;

public class LocalCharacterController : MonoBehaviour, ICharacterController {

    public int TeamIdx { get; private set; }

    public HexCoordinate HexPos =>
        HexCoordinate.ConvertFromVector3(transform.position);

    public Character Character { get; private set; }

    [SerializeField] private LocalStatus _status;
    public IStatus Status => _status;

    private IActionSelector _actionSelector;
    
    private DicePoint _dicePoint = new DicePoint();
    public IDicePoint DicePoint => _dicePoint;

    // Start is called before the first frame update
    public void Initial(string characterName, int teamIdx, bool camSync)
    {

        Character = Instantiate(CharacterManager.Instance.
            GetCharacterData(characterName).CharacterPrefab);
        Character.SetCC(this);
        Character.transform.SetParent(transform);
        Character.transform.localPosition = Vector3.zero;

        Status.SetCC(this);
        Status.CharacterCode = characterName;
        
        _dicePoint.SetCC(this);

        TeamIdx = teamIdx;
        GameManager.Instance.GameMaster.AddTeamMember(this, teamIdx);

        MapUnit mapUnit = HexGrid.Instance.GetMapUnitAt(transform.position);

        mapUnit.SetCC(gameObject, this);
    }

    public void Remove() {

        HexGrid.Instance.GetMapUnitAt(gameObject.transform.position).ResetEntityState();

        Character.Die();
        _actionSelector.Die();

        GameManager.Instance.GameMaster.RemoveTeamMember(this, TeamIdx);

    }

    public void CamSetting(string key) {
        CameraManager.Instance.CameraSetting(transform, key);
    }

    public void PlayAnim(string triggerType, string triggerValue) {
        Character.PlayAnim(triggerType, triggerValue);
    }

    public void TakeDamage(int amount)
    {
        Status.TakeDamage(amount);

    }

    public void SetPlay() {
        _dicePoint.Reset();
        ActionEnd();
    }

    public void SetActionSelector(IActionSelector actionSelector) {
        _actionSelector = actionSelector;
    }

    public void ActionEnd(float time = 0) {
        Invoke(nameof(_ActionEnd), time);
    }

    public void _ActionEnd()
    {
        if (_actionSelector == null) {
            return;
        }

        IList<IActionSelector.Action> list = Character.GetActionList();

        if (_dicePoint.IsRollDice == false)
            list.Add(IActionSelector.Action.Dice);

        _actionSelector.Ready(this, list);
    }

    public void TurnEnd() {
        GameManager.Instance.GameMaster.TurnEnd(this);
    }

    public void MoveTo(HexCoordinate before, HexCoordinate after)
    {
        HexGrid.Instance.GetMapUnitAt(before).ResetEntityState();
        HexGrid.Instance.GetMapUnitAt(after).SetCC(gameObject, this);
    }

    public void Move(Queue<Vector3> path)
    {
        Character.Move(path, MoveTo);
    }

    public void Interaction(HexCoordinate targetPos)
    {
        Character.Interaction(targetPos);
    }

    public void EquipItem(string data)
    {
        Character.EquipItem(data);

    }

}