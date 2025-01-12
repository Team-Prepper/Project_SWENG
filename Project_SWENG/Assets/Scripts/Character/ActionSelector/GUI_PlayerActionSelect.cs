using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EHTool.UIKit;

public class GUI_PlayerActionSelect : GUICustomFullScreen, IActionSelector {

    [SerializeField] private GameObject _turnEndGlowLight;
    [SerializeField] private GUI_PlayerHealth _playerHealth;
    [SerializeField] private Text _dicePoint;

    [SerializeField] RectTransform panelBtnTr;
    [SerializeField] float _openTime;

    [SerializeField] Button btnAttack;
    [SerializeField] Button btnMove;
    [SerializeField] Button btnDice;

    public Button _turnEndButton;

    ICharacterController _cc;

    public void SetPlayer(GameObject target)
    {
        CameraManager.Instance.SetCamTarget(target.transform);
        target.GetComponentInChildren<CharacterStatus>()?.SetHealthUI(_playerHealth);

    }

    public void SetCharacterController(ICharacterController cc)
    {
        _cc = cc;

    }

    public void Ready(IList<CharacterStatus.Action> actionList)
    {
        gameObject.SetActive(true);
        _cc.CamSetting("Character");

        btnDice.interactable = actionList.Contains(CharacterStatus.Action.Dice);
        btnAttack.interactable = actionList.Contains(CharacterStatus.Action.Attack);
        btnMove.interactable = actionList.Contains(CharacterStatus.Action.Move);

        StartCoroutine(_PanelOpen());
    }

    public void Die() { 
        
    }

    private IEnumerator _PanelOpen() {

        panelBtnTr.localScale = Vector2.zero;
        panelBtnTr.eulerAngles = Vector3.forward * 90f;
        panelBtnTr.gameObject.SetActive(true);

        float spendTime = 0;

        while (spendTime < _openTime)
        {
            yield return null;
            spendTime += Time.deltaTime;
            panelBtnTr.localScale = Vector2.Lerp(Vector2.zero, Vector2.one, spendTime / _openTime);
            panelBtnTr.eulerAngles = Vector3.Lerp(Vector3.forward * 90f, Vector3.zero, spendTime / _openTime);
        }

        panelBtnTr.localScale = Vector2.one;

    }

    public void OpenAttack()
    {
        new BasicTargetingAttack(_cc,  5, _cc.GetPoint());
        _AfterAction();
    }

    public void OpenMove()
    {
        UIManager.Instance.OpenGUI<GUI_Moving>("Move")?.Set(_cc);
        _AfterAction();
    }

    public void OpenDice()
    {
        UIManager.Instance.OpenGUI<GUI_Dice>("Dice").SetPlayer(_cc);
        _AfterAction();
    }

    public void TurnEndButton()
    {
        if (_nowPopUp != null) return;
        _AfterAction();
        _cc.TurnEnd();
    }

    void _AfterAction() {

        panelBtnTr.gameObject.SetActive(false);
    }

    protected override void Update()
    {
        base.Update();
        _dicePoint.text = _cc.GetPoint().ToString();
        _turnEndGlowLight.SetActive(_turnEndButton.interactable);
    }

}
