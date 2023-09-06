using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolTarget : MonoBehaviour
{
    Pool _parent;

    public void SetParentPool(Pool parent) {
        _parent = parent;
    }

    public void Return() {
        if (_parent == null)
        {
            Destroy(gameObject);
            return;
        }
        _parent.ReturnObject(gameObject);
    }
}
