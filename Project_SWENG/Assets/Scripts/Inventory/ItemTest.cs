using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemTest : MonoBehaviour
{
    [SerializeField] ItemController _testTarget;
    // Start is called before the first frame update
    private void OnEnable()
    {
        _testTarget.Pick();
    }
}
