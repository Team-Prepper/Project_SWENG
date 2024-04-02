using System.Collections.Generic;
using UnityEngine;

public interface ICharacter {
    public void Move(Queue<Vector3> currentPath);
    public void Attack(Vector3 targetPos);
    public string GetName();
}

public interface IDamagable {

    public void TakeDamage(int amount);
}