using UnityEngine;

namespace SWEng.GamePlay
{
    public abstract class ActionSelectorSpawner : MonoBehaviour
    {
        public abstract ICharacterController Spawn();
    }
}