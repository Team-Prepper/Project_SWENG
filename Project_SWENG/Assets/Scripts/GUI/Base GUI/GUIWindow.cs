using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UISystem {
    public class GUIWindow : MonoBehaviour {
        // Start is called before the first frame update

        public int priority = 0;

        private void Awake()
        {
            Open(Vector2.zero);
        }

        protected virtual void Open(Vector2 opnePos)
        {
            gameObject.SetActive(true);

            RectTransform rect = gameObject.GetComponent<RectTransform>();

            rect.SetParent(GameObject.Find("Canvas").transform);
            rect.anchoredPosition = opnePos;
            rect.localScale = Vector3.one;

        }

        public virtual void Close()
        {
            Destroy(gameObject);
        }

        public virtual void OpenWindow(string key)
        {
            UIManager.OpenGUI<GUIWindow>(key);
        }

    }

}