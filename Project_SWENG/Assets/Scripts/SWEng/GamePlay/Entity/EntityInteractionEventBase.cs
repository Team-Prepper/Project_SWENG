using UnityEngine;
using System;
using SWEng.Data;

namespace SWEng.GamePlay
{
    public abstract class EntityInteractionBase : ScriptableObject
    {
        public abstract void Interaction(ICharacter actor);
    }
}