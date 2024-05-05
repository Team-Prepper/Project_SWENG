using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CharacterSystem;
using UnityEditor.Experimental.GraphView;
using TMPro.Examples;
using System.Linq;

public class LocalCharacterController : MonoBehaviour, ICharacterController {

    Character _character;
    IActionSelector _actionSelector;


    // Start is called before the first frame update
    public void Initial(string characterName)
    {
        GameObject go = AssetOpener.ImportGameObject(characterName);
        go.transform.SetParent(transform);
        go.transform.localPosition = Vector3.zero;

        _character = go.GetComponent<Character>();
        _character.Initial(this);

        HexGrid.Instance.GetTileAt(transform.position).Entity = gameObject;

        GameManager.Instance.GameMaster.AddTeamMember(this, _character.GetTeamIdx());
    }

    public void Attack(IList<HexCoordinate> targetPos, int dmg)
    {
        _character.AttackAct(false);

        foreach (HexCoordinate hexPos in targetPos)
        {
            Hex targetHex = HexGrid.Instance.GetTileAt(hexPos);
            if (targetHex.Entity == null || !targetHex.Entity.TryGetComponent(out IDamagable target)) continue;

            target.TakeDamage(dmg);

        }

        if (targetPos.Count == 1)
        {
            transform.LookAt(targetPos.ElementAt(0).ConvertToVector3());
            CamMovement.Instance.ConvertToBattleCam();
        }
    }

    public void UseAttack() {
        _character.DoAttact();
    }
    
    public void TakeDamage(int amount)
    {
        _character.TakeDamage(amount);

        if (_character.stat.IsAlive()) {
            _character.DamageAct();
            return;
        }
        _character.DieAct();

        // 죽었을 때 GameMaster에서 처리할 것
        GameManager.Instance.GameMaster.RemoveTeamMember(this, _character.GetTeamIdx());

    }

    public void SetPlay() {
        _character.SetPlay();
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
}
