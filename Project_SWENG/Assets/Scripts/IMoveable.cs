using System.Collections.Generic;
using UnityEngine;

public interface IMoveable {
    public void Move(Queue<Vector3> currentPath);
}

public interface IDamagable {

    public void TakeDamage(int amount);
}