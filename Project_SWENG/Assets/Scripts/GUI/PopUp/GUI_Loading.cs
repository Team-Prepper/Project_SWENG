using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UISystem;
using UnityEngine.UI;

public class GUI_Loading : GUIWindow
{
    [SerializeField] Text txt;
    [SerializeField] float changeTime = 0.4f;

    [SerializeField]
    string[] ment = { "Loading", "Loading.", "Loading..", "Loading..." };

    protected override void Open(Vector2 opnePos)
    {
        base.Open(opnePos);
        StartCoroutine(Loading());
    }

    IEnumerator Loading() {
        int idx = 0;
        while (true)
        {
            txt.text = ment[idx];
            idx = (idx + 1) % ment.Length;
            yield return new WaitForSeconds(changeTime);
        }
    }
}
