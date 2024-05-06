using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CharacterSystem;
using Photon.Pun;
using System.Security.Cryptography;
using System.Linq;

public class PhotonCharacterController : MonoBehaviourPun, ICharacterController {

    [SerializeField] PhotonView _view;

    IActionSelector _actionSelector;
    Character _character;

    bool _camSync;

    public void Initial(string characterName, bool camSync)
    {
        _view.RPC("_Initial", RpcTarget.All, characterName, camSync);
        _view.RPC("_AddMember", RpcTarget.MasterClient);
    }

    [PunRPC]
    private void _Initial(string characterName, bool camSync)
    {
        GameObject go = AssetOpener.ImportGameObject(characterName);
        go.transform.SetParent(transform);
        go.transform.localPosition = Vector3.zero;

        _character = go.GetComponent<Character>();
        _character.Initial(this);

        HexGrid.Instance.GetTileAt(transform.position).Entity = gameObject;

        _camSync = camSync;
    }

    [PunRPC]
    private void _AddMember()
    {
        GameManager.Instance.GameMaster.AddTeamMember(this, _character.GetTeamIdx());

    }

    public void CamSetting() { 
        
    }

    public void Attack(IList<HexCoordinate> targetPos, int dmg, float time)
    {

        foreach (HexCoordinate hexPos in targetPos)
        {
            Hex targetHex = HexGrid.Instance.GetTileAt(hexPos);
            if (targetHex.Entity == null || !targetHex.Entity.TryGetComponent(out IDamagable target)) continue;

            target.TakeDamage(dmg);

        }
        transform.LookAt(targetPos.ElementAt(0).ConvertToVector3());
        _view.RPC("_AttackAct", RpcTarget.All, time);
    }

    [PunRPC]
    private void _AttackAct(float time)
    {
        _character.AttackAct(time);

    }

    public void DoAttack()
    {
        _character.DoAttact();
    }

    public void DoMove()
    {
        _character.DoMove();

    }


    public void TakeDamage(int amount)
    {
        _view.RPC("_TakeDamage", RpcTarget.All, amount);

    }

    [PunRPC]
    private void _TakeDamage(int amount)
    {
        _character.TakeDamage(amount);

        if (_character.stat.IsAlive())
        {
            _character.DamageAct();
            return;
        }

        _character.DieAct();
        HexGrid.Instance.GetTileAt(gameObject.transform.position).Entity = null;

        if (!PhotonNetwork.IsMasterClient) return;

        GameManager.Instance.GameMaster.RemoveTeamMember(this, _character.GetTeamIdx());

    }

    public void SetPlay()
    {
        _view.RPC("_TurnEnd", RpcTarget.MasterClient);
    }

    [PunRPC]
    private void _SetPlay() {

        if (_actionSelector == null)
        {
            return;
        }
        _character.SetPlay();
        ActionEnd();
    }

    public void SetActionSelector(IActionSelector actionSelector)
    {
        _actionSelector = actionSelector;
        _actionSelector.SetCharacterController(this);
    }


    public void ActionEnd()
    {
        _actionSelector.Ready(_character.GetCanDoAction());
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
}
