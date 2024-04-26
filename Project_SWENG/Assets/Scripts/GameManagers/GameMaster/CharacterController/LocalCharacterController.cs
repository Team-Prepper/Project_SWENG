using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CharacterSystem;

public class LocalCharacterController : MonoBehaviour, ICharacterController
{

    Character character;

    public void Attack(Vector3 targetPos, bool isSkill = false)
    {
        character.Attack(targetPos, isSkill);
    }

    public void TakeDamage(int amount)
    {
        character.TakeDamage(amount);
        
        if (character.stat.IsAlive()) return;

    }

    public void SetPlay() {
        character.SetPlay();
    }

    public void TurnEnd() {
        GameManager.Instance.GameMaster.TurnEnd(this);
    }

    // Start is called before the first frame update
    public void Initial()
    {
        character = GetComponent<Character>();
        GameManager.Instance.GameMaster.AddPlayerTeamMember(this);
    }

}
