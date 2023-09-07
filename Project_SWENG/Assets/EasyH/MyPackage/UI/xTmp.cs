using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasRenderer))]
[AddComponentMenu("UI/xTMP", 100)]
[ExecuteAlways]
public class xTmp : TextMeshProUGUI
{
    // Start is called before the first frame update
    [SerializeField] protected string m_Key = string.Empty;

    protected override void OnValidate() {
        SetText(m_Key);
        base.OnValidate();
    }

    protected override void OnEnable()
    {
        SetText(m_Key);
        StringManager.OnLangChanged.AddListener(OnLangChanged);
    }
    override protected void OnDisable()
    {
        StringManager.OnLangChanged.RemoveListener(OnLangChanged);
    }

    protected override void OnDestroy()
    {
        StringManager.OnLangChanged.RemoveListener(OnLangChanged);
    }

    public void OnLangChanged() {
        SetText(m_Key);
    }

    public void SetText(string key)
    {

        m_Key = key;

        text = StringManager.Instance.GetStringByKey(key);

        if (text.Equals(string.Empty)) {
            text = key;
        }

    }

    
}
