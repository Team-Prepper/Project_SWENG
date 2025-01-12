using UnityEngine;

public class DamageableObject : MonoBehaviour, IDamagable {
    public void TakeDamage(int amount)
    {
        Destroy(gameObject);
    }
}
