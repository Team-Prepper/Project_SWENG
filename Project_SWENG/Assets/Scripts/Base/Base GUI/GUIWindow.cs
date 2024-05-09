using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UISystem {
    public class GUIWindow : MonoBehaviour , IGUI {
        // Start is called before the first frame update

        public int priority = 0;

        public void SetOn()
        {
            gameObject.SetActive(true);
        }

        public void SetOff()
        {
            if (gameObject == null) return;
            gameObject.SetActive(false);

        }

        public virtual void Open()
        {
            gameObject.SetActive(true);

            RectTransform rect = gameObject.GetComponent<RectTransform>();

            rect.SetParent(GameObject.Find("Canvas").transform);
            rect.localScale = Vector3.one;
            rect.anchoredPosition = Vector3.zero;

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