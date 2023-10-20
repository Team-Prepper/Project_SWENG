using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UISystem {
    public class GUIFullScreen : GUIWindow {
        public LayerMask selectionMask;
        protected override void Open(Vector2 openPos)
        {
            base.Open(openPos);
            UIManager.Instance.EnrollmentGUI(this);
            gameObject.GetComponent<RectTransform>().sizeDelta = Vector3.zero;
        }

        public override void Close()
        {
            UIManager.Instance.Pop();
            base.Close();
        }

        protected virtual void Update()
        {

            if (Input.GetMouseButtonDown(0))
            {
                HexSelect(MousePointHex());
            }
        }

        public virtual void HexSelect(Vector3Int selectGridPos)
        {

        }

        public Vector3Int MousePointHex()
        {
            Vector3 touchPos = PlayerInputManager.Instance.mousePos;
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(touchPos);

            if (!Physics.Raycast(ray, out hit, 100, selectionMask)) return Vector3Int.zero;

            return HexGrid.GetClosestHex(hit.collider.gameObject.transform.position);

        }

    }
}