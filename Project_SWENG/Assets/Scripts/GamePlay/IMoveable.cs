using UnityEngine;

public interface IDamagable {

    public Transform transform { get; }
    public void TakeDamage(int amount);
}