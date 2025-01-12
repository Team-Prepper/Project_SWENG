using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EHTool.UIKit;

public class ItemController : MonoBehaviour
{
    [SerializeField] ItemData _infor;
    private float _rotateSpeed = 20;

    public void Pick() {
        UIManager.Instance.OpenGUI<GUI_ItemInterAction>("ItemInterAction").SetItem(_infor);    
    }

    private void Update()
    {
        transform.Rotate(Vector3.up * _rotateSpeed * Time.deltaTime);
    }
}
