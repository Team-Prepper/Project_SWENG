using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CharacterSystem;
using UnityEditor.Experimental.GraphView;
using TMPro.Examples;
using System.Linq;
using Photon.Pun;

public class LocalCharacterController : MonoBehaviour, ICharacterController {

    Character _character;
    IActionSelector _actionSelector;


    // Start is called before the first frame update
    public void Initial(string characterName, bool camSync)
    {
        GameObject go = AssetOpener.ImportGameObject(string.Format("Prefab/Characters/{0}", characterName));
        go.transform.SetParent(transform);
        go.transform.localPosition = Vector3.zero;

        _character = go.GetComponent<Character>();
        _character.Initial(this);

        HexGrid.Instance.GetTileAt(transform.position).Entity = gameObject;

        GameManager.Instance.GameMaster.AddTeamMember(this, _character.GetTeamIdx());
    }

    public void Attack(IList<HexCoordinate> targetPos, int dmg, float time)
    {
        foreach (HexCoordinate hexPos in targetPos)
        {
            Hex targetHex = HexGrid.Instance.GetTileAt(hexPos);
            if (targetHex.Entity == null || !targetHex.Entity.TryGetComponent(out IDamagable target)) continue;

            target.TakeDamage(dmg);

        }

        _character.AttackAct(time);

        if (targetPos.Count == 1)
        {
            transform.LookAt(targetPos.ElementAt(0).ConvertToVector3());
            CamMovement.Instance.ConvertToBattleCam();
        }
        else
        {
            transform.LookAt(targetPos.ElementAt(0).ConvertToVector3());
            CamMovement.Instance.ConvertToWideCam();

        }
    }
    public void CamSetting() {
        CamMovement.Instance.SetCamTarget(transform);
        CamMovement.Instance.ConvertToCharacterCam();
    }

    public void DoAttack() {
        _character.DoAttact();
    }

    public void DoMove()
    {
        _character.DoMove();

    }

    public void TakeDamage(int amount)
    {
        _character.TakeDamage(amount);

        if (_character.stat.IsAlive()) {
            _character.DamageAct();
            return;
        }

        _character.DieAct();
        _actionSelector.Die();
        HexGrid.Instance.GetTileAt(gameObject.transform.position).Entity = null;

        // 죽었을 때 GameMaster에서 처리할 것
        GameManager.Instance.GameMaster.RemoveTeamMember(this, _character.GetTeamIdx());

    }

    public void SetPlay() {
        _character.SetPlay();
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
        _actionSelector.Ready(_character.GetCanDoAction());
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
    public void MoveStart()
    {
        _character.MoveStart();
    }

    public void MoveEnd()
    {
        _character.MoveEnd();
    }
}
