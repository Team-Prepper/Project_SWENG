using UnityEngine;

namespace SWEng.GamePlay {

    public class DamageableObject : MonoBehaviour, IDamageable
    {
        public IStatus Status { get; private set; }

    }
}