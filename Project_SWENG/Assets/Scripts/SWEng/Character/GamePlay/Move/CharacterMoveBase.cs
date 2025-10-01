using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BKTools.Gaming.GridMap2D;

namespace SWEng
{
    public abstract class CharacterMoveBase : MonoBehaviour
    {
        public void TryAddAction(ICharacter character,
            IList<ICharacterController.Action> target)
        {
            if (character.DicePoint.GetPoint() < 2) return;

            target.Add(ICharacterController.Action.Move);
        }

        public abstract void Move(
            ICharacter character, IList<GridCoord2D> path);
    }
    
}