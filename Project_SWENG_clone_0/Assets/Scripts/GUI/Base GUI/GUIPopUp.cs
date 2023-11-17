using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UISystem {

    public class GUIPopUp : GUIWindow {
        protected override void Open(Vector2 openPos)
        {
            /*

            RectTransform rect = gameObject.GetComponent<RectTransform>();

            rect.SetParent(UIManager.Instance.NowPopUp.transform);
            */

            base.Open(openPos);

            PopUpAction();
        }

        protected void PopUpAction()
        {
            if (UIManager.Instance.NowDisplay == null)
            {
                return;
            }

            UIManager.Instance.NowDisplay.AddPopUp(this);

        }

        public override void Close()
        {
            UIManager.Instance.NowDisplay.PopPopUp();
            base.Close();
        }
    }

}