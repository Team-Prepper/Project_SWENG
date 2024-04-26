using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CharacterSystem;
using Photon.Pun;
using System.Security.Cryptography;

public class NetworkCharacterController : MonoBehaviourPun, ICharacterController {

    PhotonView _view;
    Character _character;

    public void Attack(Vector3 targetPos, bool isSkill = false)
    {
        _character.Attack(targetPos, isSkill);
    }

    public void TakeDamage(int amount)
    {
        _view.RPC("_TakeDamage", RpcTarget.All, amount);

    }

    [PunRPC]
    private void _TakeDamage(int amount)
    {
        _character.TakeDamage(amount);

        if (!PhotonNetwork.IsMasterClient) return;
        if (_character.stat.IsAlive()) return;

        // 죽었을 때 네트워크에서 처리해야 할 것들 실행

    }

    public void SetPlay() {
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

    public void Initial()
    {
        _character = GetComponent<Character>();
        _view = GetComponent<PhotonView>();

        _view.RPC("_AddMember", RpcTarget.MasterClient);
    }

    [PunRPC]
    private void _AddMember()
    {
        GameManager.Instance.GameMaster.AddPlayerTeamMember(this);

    }
}
