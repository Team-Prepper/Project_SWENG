using System.Collections.Generic;
using UnityEngine;

public class LocalCharacterController :
    MonoBehaviour, ICharacterController
{

    [SerializeField] private LocalStatus _status;
    [SerializeField] private Inventory _inventory;

    public int TeamIdx { get; private set; }

    public HexCoordinate HexPos =>
        HexCoordinate.ConvertFromVector3(transform.position);

    public Character Character { get; private set; }
    public IStatus Status => _status;
    public Inventory Inventory => _inventory;
    public IDicePoint DicePoint { get; private set; }
    public bool IsRollDice { get; set; }

    private IActionSelector _actionSelector;

    public void Initial(string characterName, int teamIdx, bool camSync)
    {
        HexGrid.Instance.GetMapUnitAt(transform.position)
            .SetCC(gameObject, this);

        Character = Instantiate(CharacterManager.Instance.
            GetCharacterData(characterName).CharacterPrefab);

        Character.SetCC(this);
        Character.AddRemoveAction(() =>
        {
            GameManager.Instance.GameMaster.
                RemoveTeamMember(this, TeamIdx);

            Destroy(gameObject);
        });

        Status.SetCC(this);
        Status.CharacterCode = characterName;

        Inventory.SetCC(this);

        DicePoint = new DicePoint();

        TeamIdx = teamIdx;

        GameManager.Instance.GameMaster.AddTeamMember(this, teamIdx);

    }

    public void Remove()
    {
        HexGrid.Instance.GetMapUnitAt(HexPos).ResetEntityState();

        Character.Die();
    }

    public void CamSetting(string key)
    {
        CameraManager.Instance.CameraSetting(transform, key);
    }

    public void PlayAnim(string triggerType, string triggerValue)
    { Character.PlayAnim(triggerType, triggerValue); }

    public void TakeDamage(int amount)
    { Status.TakeDamage(amount); }

    public void SetPlay()
    {
        if (_actionSelector == null) return;

        IsRollDice = false;
        ActionEnd();
    }

    public void SetActionSelector(IActionSelector actionSelector)
    {
        _actionSelector = actionSelector;
    }

    public void ActionEnd(float time = 0)
    {
        if (_actionSelector == null)
        {
            return;
        }

        Invoke(nameof(_ActionEnd), time);
    }

    public void _ActionEnd()
    {
        IList<IActionSelector.Action> list
            = Character.GetActionList();

        if (IsRollDice == false)
            list.Add(IActionSelector.Action.Dice);

        _actionSelector.Ready(this, list);
    }

    public void TurnEnd()
    {
        GameManager.Instance.GameMaster.TurnEnd(this);
    }

    public void MoveTo(HexCoordinate before, HexCoordinate after)
    {
        HexGrid.Instance.GetMapUnitAt(before).ResetEntityState();
        HexGrid.Instance.GetMapUnitAt(after).SetCC(gameObject, this);
    }

    public void Move(Queue<Vector3> path)
    { Character.Move(path, MoveTo); }

    public void Interaction(HexCoordinate targetPos)
    { Character.Interaction(targetPos); }

}