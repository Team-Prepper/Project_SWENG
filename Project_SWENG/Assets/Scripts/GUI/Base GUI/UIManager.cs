using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

namespace UISystem {

    public class UIManager : Singleton<UIManager> {

        List<GUIFullScreen> uiStack;

        public GUIFullScreen NowDisplay { get; private set; }

        private GUIMessageBox msgBox;
        private GUI_Dice _dice;

        public void EnrollmentGUI(GUIFullScreen newData)
        {
            if (NowDisplay == null)
            {
                NowDisplay = newData;
                return;

            }
            else
            {
                NowDisplay.gameObject.SetActive(false);
                uiStack.Add(NowDisplay);
                uiStack.Add(newData);

            }
            Pop();

        }

        public void Pop()
        {
            if (uiStack.Count < 1)
                return;

            NowDisplay = uiStack[uiStack.Count - 1];
            uiStack.RemoveAt(uiStack.Count - 1);
            NowDisplay.gameObject.SetActive(true);

        }

        class GUIData {
            internal string name;
            internal string path;

            internal void Read(XmlNode node)
            {
                name = node.Attributes["name"].Value;
                path = node.Attributes["path"].Value;
            }
        }

        Dictionary<string, GUIData> _dic;

        protected override void OnCreate()
        {
            NowDisplay = null;
            uiStack = new List<GUIFullScreen>();

            _dic = new Dictionary<string, GUIData>();
            XmlDocument xmlDoc = AssetOpener.ReadXML("GUIInfor");

            XmlNodeList nodes = xmlDoc.SelectNodes("List/Element");

            for (int i = 0; i < nodes.Count; i++)
            {
                GUIData guiData = new GUIData();
                guiData.Read(nodes[i]);

                _dic.Add(guiData.name, guiData);
            }

        }

        public static T OpenGUI<T>(string guiName)
        {
            if (Instance._dice && Instance._dice.isActiveAndEnabled)
                return default;

            string path = Instance._dic[guiName].path;
            T result = AssetOpener.Import<GameObject>(path).GetComponent<T>();

            return result;
        }

        public void DisplayMessage(string messageContent)
        {
            if (msgBox == null) msgBox = OpenGUI<GUIMessageBox>("MessageBox");
            else msgBox.gameObject.SetActive(true);

            msgBox.SetMessage(messageContent);
        }

        public void UseDice(DicePoint target)
        {
            if (_dice == null) _dice = OpenGUI<GUI_Dice>("Dice");
            else _dice.ReOpen();

            _dice.SetPlayer(target);
        }

    }
}
