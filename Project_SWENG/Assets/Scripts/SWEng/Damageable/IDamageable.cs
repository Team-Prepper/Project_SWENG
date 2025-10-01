using UnityEngine;

namespace SWEng {
    public interface IDamageable
    {
        public Transform transform { get; }
        public IStatus Status { get; }

    }
}