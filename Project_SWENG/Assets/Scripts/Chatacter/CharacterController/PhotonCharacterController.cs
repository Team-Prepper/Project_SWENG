using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CharacterSystem;
using Photon.Pun;
using System.Security.Cryptography;
using System.Linq;

public class PhotonCharacterController : MonoBehaviourPun, ICharacterController {

    IActionSelector _actionSelector;
    PhotonView _view;
    Character _character;

    public void Initial()
    {
        _character = GetComponent<Character>();
        _view = GetComponent<PhotonView>();

        _character.Initial(this);
        _view.RPC("_AddMember", RpcTarget.MasterClient);
    }

    public void ActionEnd()
    {
        //_actionSelector.SetSelector(_character.GetCanDoAction());
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

        if (!PhotonNetwork.IsMasterClient) return;

        // 죽었을 때 GameMaster에서 처리할 것

    }

    public void SetActionSelector(IActionSelector actionSelector)
    {
        _actionSelector = actionSelector;
    }

    public void SetPlay()
    {
        _character.SetPlay();
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

    [PunRPC]
    private void _AddMember()
    {
        GameManager.Instance.GameMaster.AddTeamMember(this, _character.GetTeamIdx());

    }

    public void Attack(IList<HexCoordinate> targetPos, int dmg)
    {

        foreach (HexCoordinate hexPos in targetPos)
        {
            Hex targetHex = HexGrid.Instance.GetTileAt(hexPos);
            if (targetHex.Entity == null || !targetHex.Entity.TryGetComponent(out IDamagable target)) return;

            target.TakeDamage(dmg);

        }
        transform.LookAt(targetPos.ElementAt(0).ConvertToVector3());
        _view.RPC("_AttackAct", RpcTarget.All, false);
    }

    [PunRPC]
    private void _AttackAct(bool isSkill)
    {
        _character.AttackAct(isSkill);

    }

    public void UseAttack(int idx) { }

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
