using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIFullScreen : GUIWindow
{
    protected override void Open(Vector2 openPos)
    {
        base.Open(openPos);
        UIManager.Instance.EnrollmentGUI(this);
        gameObject.GetComponent<RectTransform>().sizeDelta = Vector3.zero;
    }

    public override void Close()
    {
        UIManager.Instance.Pop();
        base.Close();
    }

}
