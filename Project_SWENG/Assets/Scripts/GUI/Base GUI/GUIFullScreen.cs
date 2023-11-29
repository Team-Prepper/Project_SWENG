using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UISystem {
    public class GUIFullScreen : GUIWindow {
        public LayerMask selectionMask;

        private List<GUIPopUp> _popupUI;
        protected GUIPopUp _nowPopUp;

        protected override void Open(Vector2 openPos)
        {
            base.Open(openPos);
            UIManager.Instance.EnrollmentGUI(this);
            gameObject.GetComponent<RectTransform>().sizeDelta = Vector3.zero;
            _popupUI = new List<GUIPopUp>();
        }

        public void AddPopUp(GUIPopUp popup) {
            if (_nowPopUp != null)
            {
                _popupUI.Add(_nowPopUp);
                _nowPopUp.gameObject.SetActive(false);
            }
            popup.gameObject.transform.SetParent(transform);
            _nowPopUp = popup;
        }

        public void PopPopUp() {
            if (_popupUI.Count == 0) {
                _nowPopUp = null;
                return;
            }
            _nowPopUp = _popupUI[_popupUI.Count - 1];
            _nowPopUp.gameObject.SetActive(true);
            _popupUI.RemoveAt(_popupUI.Count - 1);
        }

        public override void Close()
        {
            UIManager.Instance.Pop();
            base.Close();
        }

        protected virtual void Update()
        {
            if (_nowPopUp) return;

            if (Input.GetMouseButtonUp(0))
            {
                HexCoordinate coord = MousePointHex();

                //Debug.Log(coord);
                HexSelect(coord);
            }
        }

        public virtual void HexSelect(HexCoordinate selectGridPos)
        {

        }

        public HexCoordinate MousePointHex()
        {
            Vector3 touchPos = PlayerInputManager.Instance.mousePos;
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(touchPos);

            if (!Physics.Raycast(ray, out hit, 100, selectionMask)) return new HexCoordinate(0, 0);

            return HexCoordinate.ConvertFromVector3(hit.collider.gameObject.transform.position);

        }

    }
}