using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UISystem {

    public class UIManager : Singleton<UIManager> {

        class GUIData {
            internal string name;
            internal string path;

            internal void Read(XmlNode node)
            {
                name = node.Attributes["name"].Value;
                path = node.Attributes["path"].Value;
            }
        }

        IDictionary<string, GUIData> _dic;
        IList<IGUIFullScreen> uiStack;

        public IGUIFullScreen NowDisplay { get; private set; }

        private GUIMessageBox msgBox;
        private GUI_Dice _dice;

        public void EnrollmentGUI(IGUIFullScreen newData)
        {
            if (NowDisplay == null)
            {
                NowDisplay = newData;
                return;

            }
            else
            {
                NowDisplay.SetOff();
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
            NowDisplay.SetOn();

        }

        protected override void OnCreate()
        {
            NowDisplay = null;
            uiStack = new List<IGUIFullScreen>();

            _dic = new Dictionary<string, GUIData>();
            XmlDocument xmlDoc = AssetOpener.ReadXML("GUIInfor");

            XmlNodeList nodes = xmlDoc.SelectNodes("List/Element");

            for (int i = 0; i < nodes.Count; i++)
            {
                GUIData guiData = new GUIData();
                guiData.Read(nodes[i]);

                _dic.Add(guiData.name, guiData);
            }

            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            NowDisplay = null;
            uiStack = new List<IGUIFullScreen>();

        }

        public static T OpenGUI<T>(string guiName)
        {
            if (Instance._dice && Instance._dice.isActiveAndEnabled)
                return default;

            string path = Instance._dic[guiName].path;

            GameObject retGO = AssetOpener.Import<GameObject>(path);
            retGO.GetComponent<IGUI>().Open();

            return retGO.GetComponent<T>();
        }

        public void DisplayMessage(string messageContent)
        {
            if (msgBox == null) msgBox = OpenGUI<GUIMessageBox>("MessageBox");
            else msgBox.SetOn();

            msgBox.SetMessage(messageContent);
        }

        public void UseDice(IDicePoint target)
        {
            if (_dice == null) _dice = OpenGUI<GUI_Dice>("Dice");
            else _dice.ReOpen();

            _dice.SetPlayer(target);
        }

    }
}
