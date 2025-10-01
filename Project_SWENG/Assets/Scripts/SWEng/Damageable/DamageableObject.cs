using UnityEngine;

namespace SWEng {

    public class DamageableObject : MonoBehaviour, IDamageable
    {
        public IStatus Status { get; private set; }

    }
}