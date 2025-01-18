using System.Collections.Generic;
using UnityEngine;
using EHTool;

public class LocalCharacterController : MonoBehaviour, ICharacterController {

    public int TeamIdx { get; private set; }

    public HexCoordinate HexPos =>
        HexCoordinate.ConvertFromVector3(transform.position);

    public Character Character { get; private set; }
    public bool RollDice { get; set; }

    IActionSelector _actionSelector;

    // Start is called before the first frame update
    public void Initial(string characterName, int teamIdx, bool camSync)
    {
        Debug.Log(characterName);

        GameObject go = AssetOpener.ImportGameObject(string.Format("Prefab/Characters/{0}", characterName));

        go.transform.SetParent(transform);
        go.transform.localPosition = Vector3.zero;

        Character = go.GetComponent<Character>();

        if (Character == null)
            Character = go.AddComponent<Character>();

        Character.Initial(this);

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
        Character.PlayAnim(triggerType, triggerValue);
    }

    public void TakeDamage(int amount)
    {
        Character.Status.TakeDamage(amount);

        if (Character.Status.IsAlive)
        {
            PlayAnim("SetTrigger", "Hit");
            return;
        }

        HexGrid.Instance.GetMapUnitAt(gameObject.transform.position).SetEntity(null);

        PlayAnim("SetTrigger", "Die");
        _actionSelector.Die();

        GameManager.Instance.GameMaster.RemoveTeamMember(this, TeamIdx);

    }

    public void SetPlay() {
        RollDice = false;
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

        _actionSelector.Ready(Character.GetActionList());
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
        Character.Move(path);
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