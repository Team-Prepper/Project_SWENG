using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : MonoBehaviour, IDamagable {
    public void TakeDamage(int amount)
    {
        Destroy(gameObject);
    }
}
