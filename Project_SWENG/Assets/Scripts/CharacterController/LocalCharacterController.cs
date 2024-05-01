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

    public void Attack(IList<HexCoordinate> targetPos, int dmg)
    {
        transform.LookAt(targetPos.ElementAt(0).ConvertToVector3());
        _character.AttackAct(false);

        Debug.Log(targetPos.Count);

        foreach (HexCoordinate hexPos in targetPos)
        {
            Hex targetHex = HexGrid.Instance.GetTileAt(hexPos);
            if (targetHex.Entity == null || !targetHex.Entity.TryGetComponent(out IDamagable target)) return;

            target.TakeDamage(dmg);

        }
    }

    public void UseAttack(int idx) {
        _character.DoAttact(idx);
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
    }

    public void SetActionSelector(IActionSelector actionSelector) {
        _actionSelector = actionSelector;
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

    // Start is called before the first frame update
    public void Initial()
    {
        _character = GetComponent<Character>();
        _character.Initial(this);
        GameManager.Instance.GameMaster.AddTeamMember(this, _character.GetTeamIdx());
    }

}
