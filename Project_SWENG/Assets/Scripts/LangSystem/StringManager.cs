using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using UnityEngine.Events;

namespace LangSystem {
    public class StringManager : Singleton<StringManager> {
        // Start is called before the first frame update

        public static UnityEvent OnLangChanged = new UnityEvent();
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
            ReadStringFromXml(_nowLang);
        }

        public void ReadStringFromXml(string lang)
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
            ReadStringFromXml(_nowLang);
            OnLangChanged.Invoke();

        }

        public void ChangeLang(string lang)
        {
            ReadStringFromXml(lang);
            OnLangChanged.Invoke();
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