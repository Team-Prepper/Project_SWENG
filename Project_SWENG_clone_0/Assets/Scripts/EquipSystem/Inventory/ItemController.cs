using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UISystem;

public class ItemController : MonoBehaviour
{
    [SerializeField] Item _infor;
    private float _rotateSpeed = 20;

    public void Pick() {
        UIManager.OpenGUI<GUI_ItemInterAction>("ItemInterAction").SetItem(_infor);    
    }

    private void Update()
    {
        transform.Rotate(Vector3.up * _rotateSpeed * Time.deltaTime);
    }
}
