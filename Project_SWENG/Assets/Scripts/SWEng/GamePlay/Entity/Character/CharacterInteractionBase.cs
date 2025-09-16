using System;
using System.Collections;
using UnityEngine;

namespace SWEng.GamePlay
{
    public abstract class CharacterInteractionBase : EntityInteractionBase
    {
        protected ICharacter _target;

        public void SetData(ICharacter character)
        {
            _target = character;
        }
    }
    
}