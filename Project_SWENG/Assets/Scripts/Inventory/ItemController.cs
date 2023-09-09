using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemController : MonoBehaviour
{
    [SerializeField] Item _infor;

    public void Pick() {
        UIManager.OpenGUI<GUI_ItemInterAction>("ItemInterAction").SetItem(_infor);    
    }

}
