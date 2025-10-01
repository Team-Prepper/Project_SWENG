using UnityEngine;

namespace SWEng
{
    public abstract class ActionSelectorSpawner : MonoBehaviour
    {
        public abstract ICharacterController Spawn();
    }
}