using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceCam : MonoBehaviour
{
    [SerializeField] private GameObject dice;

    private void Update()
    {
        transform.localPosition = new Vector3(dice.transform.localPosition.x, 15, dice.transform.localPosition.z);
    }
}
