using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using EHTool;

public class PhotonCharacterController : MonoBehaviourPun, ICharacterController {

    public int TeamIdx { get; private set; }

    public HexCoordinate HexPos =>
        HexCoordinate.ConvertFromVector3(transform.position);

    public Character Character { get; private set; }
    public bool RollDice { get; set; }

    [SerializeField] PhotonView _view;

    IActionSelector _actionSelector;
    bool _camSync;

    public void Initial(string characterName, int teamIdx, bool camSync)
    {
        _view.RPC("_Initial", RpcTarget.All, characterName, teamIdx, camSync);
        _view.RPC("_AddMember", RpcTarget.MasterClient, teamIdx);
    }

    [PunRPC]
    private void _Initial(string characterName, int teamIdx, bool camSync)
    {
        GameObject go = AssetOpener.ImportGameObject(string.Format("Prefab/Characters/{0}", characterName));

        go.transform.SetParent(transform);
        go.transform.localPosition = Vector3.zero;

        Character = go.GetComponent<Character>();

        if (Character == null)
            Character = go.AddComponent<Character>();

        Character.Initial(this);

        HexGrid.Instance.GetMapUnitAt(transform.position).SetCC(gameObject, this);

        TeamIdx = teamIdx;
        _camSync = camSync;
    }

    [PunRPC]
    private void _AddMember(int teamIdx)
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

        _view.RPC("_CamSetting", RpcTarget.All, key);

    }

    [PunRPC]
    private void _CamSetting(string key)
    {
        CameraManager.Instance.CameraSetting(transform, key);

    }

    public void PlayAnim(string triggerType, string triggerValue)
    {
        _view.RPC("_PlayAnim", RpcTarget.All, triggerType, triggerValue);
    }

    [PunRPC]
    public void _PlayAnim(string triggerType, string triggerValue)
    {
        Character.PlayAnim(triggerType, triggerValue);

    }

    public void TakeDamage(int amount)
    {
        _view.RPC("_TakeDamage", RpcTarget.All, amount);

    }

    [PunRPC]
    private void _TakeDamage(int amount)
    {
        Character.Status.TakeDamage(amount);

        if (Character.Status.IsAlive)
        {
            Character.PlayAnim("SetTrigger", "Hit");
            return;
        }

        HexGrid.Instance.GetMapUnitAt(HexPos).ResetEntityState();

        if (!PhotonNetwork.IsMasterClient) return;

        PlayAnim("SetTrigger", "Die");
        _actionSelector.Die();
        Character.Die();

        GameManager.Instance.GameMaster.RemoveTeamMember(this, TeamIdx);

    }

    public void SetPlay()
    {
        _view.RPC("_SetPlay", RpcTarget.All);
    }

    [PunRPC]
    private void _SetPlay() {

        if (_actionSelector == null)
        {
            return;
        }
        RollDice = false;
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

        _actionSelector.Ready(Character.GetActionList());
    }

    public void TurnEnd()
    {
        _view.RPC("_TurnEnd", RpcTarget.MasterClient);
    }

    [PunRPC]
    private void _TurnEnd()
    {
        GameManager.Instance.GameMaster.TurnEnd(this);
    }

    public void MoveTo(HexCoordinate before, HexCoordinate after)
    {
        _view.RPC("_MoveTo", RpcTarget.All, before.x, before.z, after.x, after.z);
    }

    [PunRPC]
    private void _MoveTo(int beforeX, int beforeZ, int afterX, int afterZ) {

        HexCoordinate before = new HexCoordinate(beforeX, beforeZ);
        HexCoordinate after = new HexCoordinate(afterX, afterZ);

        HexGrid.Instance.GetMapUnitAt(before).ResetEntityState();
        HexGrid.Instance.GetMapUnitAt(after).SetCC(gameObject, this);

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