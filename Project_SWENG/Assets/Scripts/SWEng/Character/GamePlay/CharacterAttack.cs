using UnityEngine;
using System.Collections.Generic;

namespace SWEng {

    public class CharacterAttack : MonoBehaviour
    {

        [SerializeField] private int _usePointAtAttack = 3;

        public void TryAddAction(ICharacter character,
            IList<ICharacterController.Action> target)
        {
            if (character.DicePoint.GetPoint() < _usePointAtAttack) return;

            target.Add(ICharacterController.Action.Attack);
        }

    }
}