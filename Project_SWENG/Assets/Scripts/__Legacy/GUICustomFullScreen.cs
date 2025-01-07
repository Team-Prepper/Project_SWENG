using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EHTool.UIKit;

public class GUICustomFullScreen : GUIWindow, IGUIFullScreen {

    public LayerMask selectionMask;

    private IList<IGUIPopUp> _popupUI;
    protected IGUIPopUp _nowPopUp;

    public void AddPopUp(IGUIPopUp popUp)
    {
        if (_nowPopUp != null)
        {
            _popupUI.Add(_nowPopUp);
            _nowPopUp.SetOff();
        }
        _nowPopUp = popUp;

    }

    public override void Open()
    {
        base.Open();
        UIManager.Instance.OpenFullScreen(this);
        gameObject.GetComponent<RectTransform>().sizeDelta = Vector3.zero;
        _popupUI = new List<IGUIPopUp>();
    }

    public void AddPopUp(GUIPopUp popup)
    {
    }

    public void ClosePopUp(IGUIPopUp popUp)
    {
        if (_popupUI.Count == 0)
        {
            _nowPopUp = null;
            return;
        }

        _nowPopUp = _popupUI[_popupUI.Count - 1];
        _nowPopUp.SetOn();
        _popupUI.RemoveAt(_popupUI.Count - 1);
    }

    public void AddPanel(IGUIPanel panel)
    {
        throw new System.NotImplementedException();
    }

    public void ClosePanel()
    {
        throw new System.NotImplementedException();
    }

    public override void Close()
    {
        UIManager.Instance.CloseFullScreen(this);

        while (_popupUI.Count > 0)
        {
            _nowPopUp.Close();
        }

        base.Close();
    }

    protected virtual void Update()
    {
        if (_nowPopUp != null) return;

        if (Input.GetMouseButtonUp(0))
        {
            HexCoordinate coord = MousePointHex();

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