using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PhotonCharacterController : MonoBehaviourPun, ICharacterController {

    public int TeamIdx { get; private set; }

    public HexCoordinate HexPos =>
        HexCoordinate.ConvertFromVector3(transform.position);

    public Character Character { get; private set; }

    private DicePoint _dicePoint = new DicePoint();
    public IDicePoint DicePoint => _dicePoint;

    [SerializeField] private PhotonStatus _status;
    public IStatus Status => _status;

    [SerializeField] private PhotonView _view;

    private IActionSelector _actionSelector;
    private bool _camSync;

    public void Initial(string characterName, int teamIdx, bool camSync)
    {
        _view.RPC("PunAllInitial", RpcTarget.All, characterName, teamIdx, camSync);
        _view.RPC("PunMasterAddMember", RpcTarget.MasterClient, teamIdx);
    }

    [PunRPC]
    private void PunAllInitial(string characterName, int teamIdx, bool camSync)
    {
        Character = Instantiate(CharacterManager.Instance.GetCharacterData(characterName).CharacterPrefab);
        Character.SetCC(this);
        Character.transform.SetParent(transform);
        Character.transform.localPosition = Vector3.zero;

        Status.SetCC(this);
        Status.CharacterCode = characterName;
        
        _dicePoint.SetCC(this);

        HexGrid.Instance.GetMapUnitAt(transform.position).SetCC(gameObject, this);

        TeamIdx = teamIdx;
        _camSync = camSync;
    }

    [PunRPC]
    private void PunMasterAddMember(int teamIdx)
    {
        TeamIdx = teamIdx;
        GameManager.Instance.GameMaster.AddTeamMember(this, TeamIdx);

    }

    public void CamSetting(string key)
    {
        if (!_camSync)
        {
            CameraManager.Instance.CameraSetting(transform, key);
            return;
        }

        _view.RPC("PunAllCamSetting", RpcTarget.All, key);

    }

    [PunRPC]
    private void PunAllCamSetting(string key)
    {
        CameraManager.Instance.CameraSetting(transform, key);
    }

    public void PlayAnim(string triggerType, string triggerValue)
    {
        _view.RPC("PunAllPlayAnim", RpcTarget.All, triggerType, triggerValue);
    }

    [PunRPC]
    public void PunAllPlayAnim(string triggerType, string triggerValue)
    {
        Character.PlayAnim(triggerType, triggerValue);

    }

    public void Remove() {
        _view.RPC("PunMasterRemove", RpcTarget.MasterClient);
        
    }

    [PunRPC]
    public void PunMasterRemove() {
        
        HexGrid.Instance.GetMapUnitAt(HexPos).ResetEntityState();

        _actionSelector.Die();
        Character.Die();

        GameManager.Instance.GameMaster.RemoveTeamMember(this, TeamIdx);

    }

    public void TakeDamage(int amount)
    {
        Status.TakeDamage(amount);
    }

    public void SetPlay()
    {
        _view.RPC("PunAllSetPlay", RpcTarget.All);
    }

    [PunRPC]
    private void PunAllSetPlay() {

        if (_actionSelector == null)
        {
            return;
        }
        _dicePoint.Reset();
        ActionEnd();
    }

    public void SetActionSelector(IActionSelector actionSelector)
    {
        _actionSelector = actionSelector;
        _actionSelector.SetCharacterController(this);
    }

    public void ActionEnd(float time = 0)
    {
        Invoke(nameof(_ActionEnd), time);
    }

    public void _ActionEnd()
    {
        if (_actionSelector == null)
        {
            return;
        }

        IList<IActionSelector.Action> list = Character.GetActionList();

        if (_dicePoint.IsRollDice == false)
            list.Add(IActionSelector.Action.Dice);
        _actionSelector.Ready(list);
        
    }

    public void TurnEnd()
    {
        _view.RPC("PunMasterTurnEnd", RpcTarget.MasterClient);
    }

    [PunRPC]
    private void PunMasterTurnEnd()
    {
        GameManager.Instance.GameMaster.TurnEnd(this);
    }

    public void MoveTo(HexCoordinate before, HexCoordinate after)
    {
        _view.RPC("PunAllMoveTo", RpcTarget.All, before.x, before.z, after.x, after.z);
    }

    [PunRPC]
    private void PunAllMoveTo(int beforeX, int beforeZ, int afterX, int afterZ) {

        HexCoordinate before = new HexCoordinate(beforeX, beforeZ);
        HexCoordinate after = new HexCoordinate(afterX, afterZ);

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