using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using EHTool;

public class PhotonCharacterController : MonoBehaviourPun, ICharacterController {

    public int TeamIdx { get; private set; }
    public HexCoordinate HexPos => HexCoordinate.ConvertFromVector3(transform.position);

    [SerializeField] PhotonView _view;

    IActionSelector _actionSelector;

    [SerializeField] protected int _dicePoint;

    CharacterStatus _status;
    CharacterMove _moveComp;
    CharacterAttack _attackComp;

    [SerializeField] bool _rollDice = false;
    bool _camSync;

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

    public void Initial(string characterName, int teamIdx, bool camSync)
    {
        _view.RPC("_Initial", RpcTarget.All, characterName, teamIdx, camSync);
        _view.RPC("_AddMember", RpcTarget.MasterClient);
    }

    [PunRPC]
    private void _Initial(string characterName, int teamIdx, bool camSync)
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

        _camSync = camSync;
    }

    [PunRPC]
    private void _AddMember()
    {
        GameManager.Instance.GameMaster.AddTeamMember(this, TeamIdx);

    }

    public void CamSetting(string key)
    {
        if (!_camSync)
        {
            CameraManager.Instance.ConverTo(transform, key);
            return;
        }

        _view.RPC("_CamSetting", RpcTarget.All, key);

    }

    [PunRPC]
    private void _CamSetting(string key)
    {
        CameraManager.Instance.ConverTo(transform, key);

    }

    public void PlayAnim(string triggerType, string triggerValue)
    {
        _view.RPC("_PlayAnim", RpcTarget.All, triggerType, triggerValue);
    }

    [PunRPC]
    public void _PlayAnim(string triggerType, string triggerValue)
    {
        _status.PlayAnim(triggerType, triggerValue);

    }

    public void TakeDamage(int amount)
    {
        _view.RPC("_TakeDamage", RpcTarget.All, amount);

    }

    [PunRPC]
    private void _TakeDamage(int amount)
    {
        _status.TakeDamage(amount);

        if (_status.IsAlive)
        {
            _PlayAnim("SetTrigger", "Hit");
            return;
        }

        HexGrid.Instance.GetTileAt(gameObject.transform.position).Entity = null;

        if (!PhotonNetwork.IsMasterClient) return;

        PlayAnim("SetTrigger", "Die");
        _actionSelector.Die();

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
        _rollDice = false;
        ActionEnd();
    }

    public void SetActionSelector(IActionSelector actionSelector)
    {
        _actionSelector = actionSelector;
        _actionSelector.SetCharacterController(this);
    }


    public void ActionEnd()
    {
        if (_actionSelector == null)
        {
            return;
        }

        List<CharacterStatus.Action> list = new List<CharacterStatus.Action>();

        if (_rollDice == false) list.Add(CharacterStatus.Action.Dice);

        _moveComp.TryAddAction(list);
        _attackComp.TryAddAction(list);

        _actionSelector.Ready(list);
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

        HexGrid.Instance.GetTileAt(before).Entity = null;
        HexGrid.Instance.GetTileAt(after).Entity = gameObject;
        HexGrid.Instance.GetTileAt(after).CloudActiveFalse();
    }

    public void Move(Queue<Vector3> path)
    {
        _moveComp.Move(path);
    }

}
