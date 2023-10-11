using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;


namespace LangSystem {

    [RequireComponent(typeof(CanvasRenderer))]
    [AddComponentMenu("UI/xTMP", 100)]
    [ExecuteAlways]
    public class xTmp : TextMeshProUGUI, IStringListener {

        [SerializeField] string m_Key = string.Empty;

        protected override void OnValidate()
        {
            SetText(m_Key);
            base.OnValidate();
        }

        protected override void OnEnable()
        {
            StringManager.Instance.AddListner(this);
            SetText(m_Key);
            base.OnEnable();
        }

        override protected void OnDisable()
        {
            StringManager.Instance.RemoveListner(this);
            base.OnDisable();
        }

        protected override void OnDestroy()
        {
            StringManager.Instance.RemoveListner(this);
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
}
