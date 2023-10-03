using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIPopUp : GUIWindow
{
    protected override void Open(Vector2 openPos)
    {
        /*
        if (UIManager.Instance.NowPopUp == null)
        {
            base.Open();
            return;
        }

        RectTransform rect = gameObject.GetComponent<RectTransform>();

        rect.SetParent(UIManager.Instance.NowPopUp.transform);
        */
        base.Open(openPos);
    }
}
