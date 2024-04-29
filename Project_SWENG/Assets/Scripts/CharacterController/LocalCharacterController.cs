using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CharacterSystem;
using UnityEditor.Experimental.GraphView;

public class LocalCharacterController : MonoBehaviour, ICharacterController
{

    Character _character;

    public void Attack(Vector3 targetPos, bool isSkill = false)
    {
        transform.LookAt(targetPos);
        _character.AttackAct(isSkill);

        Hex targetHex = HexGrid.Instance.GetTileAt(targetPos);
        if (targetHex.Entity == null || !targetHex.Entity.TryGetComponent(out IDamagable target)) return;

        int totalDmg = _character.GetAttackValue();
        target.TakeDamage(totalDmg);
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
