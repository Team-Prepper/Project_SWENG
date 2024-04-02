using System.Collections.Generic;
using UnityEngine;

public interface IMoveable {
    public void Move(Queue<Vector3> currentPath);
    public string GetName();
}

public interface IAttackable {
    public void Attack(Vector3 targetPos);

}

public interface IDamagable {

    public void TakeDamage(int amount);
}