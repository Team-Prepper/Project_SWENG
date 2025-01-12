using System.Collections.Generic;
using UnityEngine;
using EHTool;

public class LocalCharacterController : MonoBehaviour, ICharacterController {

    public int TeamIdx { get; private set; }

    IActionSelector _actionSelector;
    public HexCoordinate HexPos => HexCoordinate.ConvertFromVector3(transform.position);

    [SerializeField] int _dicePoint = 0;

    CharacterStatus _status;
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
        GameObject go = AssetOpener.ImportGameObject(string.Format("Prefab/Characters/{0}", characterName));

        go.transform.SetParent(transform);
        go.transform.localPosition = Vector3.zero;

        _status = go.GetComponent<CharacterStatus>();
        _moveComp = go.GetComponent<CharacterMove>();
        _attackComp = go.GetComponent<CharacterAttack>();
        
        if (_moveComp == null)
            _moveComp = gameObject.AddComponent<CharacterMove>();
        if (_attackComp == null)
            _attackComp = gameObject.AddComponent<CharacterAttack>();

        _status.SetCC(this);
        _moveComp.SetCC(this);
        _attackComp.SetCC(this);

        HexGrid.Instance.GetTileAt(transform.position).Entity = gameObject;

        TeamIdx = teamIdx;
        GameManager.Instance.GameMaster.AddTeamMember(this, teamIdx);
    }

    public void CamSetting(string key) {
        CameraManager.Instance.ConverTo(transform, key);
    }

    public void PlayAnim(string triggerType, string triggerValue) {
        _status.PlayAnim(triggerType, triggerValue);
    }

    public void TakeDamage(int amount)
    {
        _status.TakeDamage(amount);

        if (_status.IsAlive)
        {
            PlayAnim("SetTrigger", "Hit");
            return;
        }

        HexGrid.Instance.GetTileAt(gameObject.transform.position).Entity = null;

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

    public void ActionEnd()
    {
        if (_actionSelector == null) {
            return;
        }

        List<CharacterStatus.Action> list = new List<CharacterStatus.Action>();

        if (_rollDice == false) list.Add(CharacterStatus.Action.Dice);

        _moveComp.TryAddAction(list);
        _attackComp.TryAddAction(list);

        _actionSelector.Ready(list);
    }

    public void TurnEnd() {
        GameManager.Instance.GameMaster.TurnEnd(this);
    }

    public void MoveTo(HexCoordinate before, HexCoordinate after)
    {
        HexGrid.Instance.GetTileAt(before).Entity = null;
        HexGrid.Instance.GetTileAt(after).Entity = gameObject;
        HexGrid.Instance.GetTileAt(after).CloudActiveFalse();
    }

    public void Move(Queue<Vector3> path)
    {
        _moveComp.Move(path);
    }
}