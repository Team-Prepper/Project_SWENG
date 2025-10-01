using System.Collections.Generic;
using System;

namespace SWEng
{

    public interface ICharacterController
    {
        public enum Action
        {
            Dice, Move, Attack, TurnEnd,
            Interaction
        }

        public void Ready(ICharacter cc, IList<Action> actionList);
        public void SelectAttackPoint(Action<IList<IDamageable>> action);
        
    }
    
}