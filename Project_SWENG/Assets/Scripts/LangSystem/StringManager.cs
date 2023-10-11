using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using UnityEngine.Events;
using Unity.VisualScripting;
using UnityEngine.InputSystem;

namespace LangSystem {

    public interface IStringListener {

        public void OnLangChanged();

        public void SetText(string key);

    }

    public class StringManager : Singleton<StringManager> {
        // Start is called before the first frame update

        List<IStringListener> _targets;

        public void AddListner(IStringListener element) {
            _targets.Add(element);
        }

        public void RemoveListner(IStringListener element)
        {
            _targets.Add(element);

        }

        class StringData {
            public string key;
            public string value;

            public void Read(XmlNode node)
            {
                key = node.Attributes["key"].Value;
                value = node.Attributes["value"].Value;
            }
        }

        Dictionary<string, string> _dic;

        string _nowLang = "Korean";

        protected override void OnCreate()
        {
            _targets = new List<IStringListener>();
            _ReadStringFromXml(_nowLang);
        }

        private void _ReadStringFromXml(string lang)
        {
            _dic = new Dictionary<string, string>();
            XmlDocument xmlDoc = AssetOpener.ReadXML("String/" + _nowLang);

            XmlNodeList nodes = xmlDoc.SelectNodes("List/Element");

            for (int i = 0; i < nodes.Count; i++)
            {
                StringData stringData = new StringData();
                stringData.Read(nodes[i]);

                _dic.Add(stringData.key, stringData.value);
            }

        }

        public void UpdateData()
        {
            _ReadStringFromXml(_nowLang);
            _LangChange();
        }

        public void ChangeLang(string lang)
        {
            _ReadStringFromXml(lang);
            _LangChange();
        }

        private void _LangChange() {
            foreach (IStringListener target in _targets) {
                target.OnLangChanged();
            }
        }

        public string GetStringByKey(string key)
        {

            if (_dic.TryGetValue(key, out string value))
            {
                return value;
            }
            return key;

        }
    }

}