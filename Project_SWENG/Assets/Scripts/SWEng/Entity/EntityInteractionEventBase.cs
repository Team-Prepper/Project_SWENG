using UnityEngine;

namespace SWEng
{
    public abstract class EntityInteractionBase : ScriptableObject
    {
        public abstract void Interaction(ICharacter actor);
    }
}