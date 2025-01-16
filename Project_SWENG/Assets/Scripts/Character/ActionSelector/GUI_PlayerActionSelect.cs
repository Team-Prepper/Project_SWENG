using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EHTool.UIKit;

public class GUI_PlayerActionSelect : GUICustomFullScreen, IActionSelector {

    [SerializeField] private GameObject _turnEndGlowLight;
    [SerializeField] private GUI_PlayerHealth _playerHealth;
    [SerializeField] private Text _dicePoint;

    [SerializeField] RectTransform _panelBtnTr;
    [SerializeField] float _openTime;

    [SerializeField] private Button _btnInteraction;
    [SerializeField] private Button _btnAttack;
    [SerializeField] private Button _btnMove;
    [SerializeField] private Button _btnDice;

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

    public void Ready(IList<IActionSelector.Action> actionList)
    {
        gameObject.SetActive(true);
        _cc.CamSetting("Character");

        _btnInteraction.interactable = actionList.Contains(IActionSelector.Action.Interaction);
        _btnDice.interactable = actionList.Contains(IActionSelector.Action.Dice);
        _btnAttack.interactable = actionList.Contains(IActionSelector.Action.Attack);
        _btnMove.interactable = actionList.Contains(IActionSelector.Action.Move);

        StartCoroutine(_PanelOpen());
    }

    public void Die() { 
        
    }

    private IEnumerator _PanelOpen() {

        _panelBtnTr.localScale = Vector2.zero;
        _panelBtnTr.eulerAngles = Vector3.forward * 90f;
        _panelBtnTr.gameObject.SetActive(true);

        float spendTime = 0;

        while (spendTime < _openTime)
        {
            yield return null;
            spendTime += Time.deltaTime;
            _panelBtnTr.localScale = Vector2.Lerp(Vector2.zero, Vector2.one, spendTime / _openTime);
            _panelBtnTr.eulerAngles = Vector3.Lerp(Vector3.forward * 90f, Vector3.zero, spendTime / _openTime);
        }

        _panelBtnTr.localScale = Vector2.one;

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

    public void OpenInteraction()
    {
        UIManager.Instance.OpenGUI<GUIInteraction>("Interaction").Set(_cc);
        _AfterAction();

    }

    public void TurnEndButton()
    {
        if (_nowPopUp != null) return;
        _AfterAction();
        _cc.TurnEnd();
    }

    void _AfterAction() {

        _panelBtnTr.gameObject.SetActive(false);
    }

    protected override void Update()
    {
        base.Update();
        _dicePoint.text = _cc.GetPoint().ToString();
        _turnEndGlowLight.SetActive(_turnEndButton.interactable);
    }

}
