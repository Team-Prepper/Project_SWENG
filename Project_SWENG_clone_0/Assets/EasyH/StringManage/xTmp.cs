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
    [SerializeField] protected string m_Key = string.Empty;
    protected override void OnValidate()
    {
        SetText(m_Key);
        base.OnValidate();
    }

    protected override void OnEnable()
    {
        StringManager.OnLangChanged.AddListener(OnLangChanged);
        SetText(m_Key);
        base.OnEnable();
    }
    override protected void OnDisable()
    {
        StringManager.OnLangChanged.RemoveListener(OnLangChanged);
        base.OnDisable();
    }

    protected override void OnDestroy()
    {
        StringManager.OnLangChanged.RemoveListener(OnLangChanged);
        base.OnDestroy();
    }

    public void OnLangChanged()
    {
        SetText(m_Key);
    }

    public void SetText(string key)
    {

        m_Key = key;

        text = StringManager.Instance.GetStringByKey(key);

        if (text.Equals(string.Empty))
        {
            text = key;
        }

    }


}
