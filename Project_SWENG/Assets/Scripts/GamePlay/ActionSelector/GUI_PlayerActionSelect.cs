using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EHTool.UIKit;
using System;
using System.Linq;

public class GUI_PlayerActionSelect : GUICustomFullScreen, IActionSelector {

    [SerializeField] private BarHPBar _playerHealth;
    [SerializeField] private Text _dicePoint;

    [SerializeField] private RectTransform _panelBtnTr;
    [SerializeField] private float _openTime;

    [SerializeField] private Button _btnInteraction;
    [SerializeField] private Button _btnAttack;
    [SerializeField] private Button _btnMove;
    [SerializeField] private Button _btnDice;

    private IDictionary<ICharacterController, IList<IActionSelector.Action>> _todoList
        = new Dictionary<ICharacterController, IList<IActionSelector.Action>>();
    private ICharacterController _cc;

#nullable enable
    private IDisposable? _disposable;

    public void SetCharacterController(ICharacterController cc)
    {
        _cc = cc;
        _disposable?.Dispose();
        _disposable = _cc.Status.Subscribe(_playerHealth);
        
    }

    public void Ready(ICharacterController cc, IList<IActionSelector.Action> actionList)
    {
        gameObject.SetActive(true);

        if (_cc == null) {
            SetCharacterController(cc);
        }
        if (_cc == cc) {
            Func(actionList);
            return;
        }

        _todoList.Add(cc, actionList);

    }

    private void Func(IList<IActionSelector.Action> actionList) {

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

    public void SelectTarget(ISkill attack) {

        UIManager.Instance.OpenGUI<GUI_AttackSelect>
            ("AttackSelect").Set(attack, _cc);

    }

    public void OpenAttack()
    {
        _cc.Status.Skill.GetSkill().Set(this, _cc);
        _AfterAction();
    }

    public void OpenMove()
    {
        UIManager.Instance.OpenGUI<GUI_Moving>("Move")?.Set(_cc);
        _AfterAction();
    }

    public void OpenDice()
    {
        UIManager.Instance.OpenGUI<GUI_Dice>("Dice").SetPlayer(_cc.DicePoint);
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
        _cc = null;

        if (_todoList.Count < 1) return;

        SetCharacterController(_todoList.ToList()[0].Key);
        Func(_todoList[_cc]);
        _todoList.Remove(_cc);
    }

    void _AfterAction() {

        _panelBtnTr.gameObject.SetActive(false);
    }

    protected override void Update()
    {
        base.Update();
        if (_cc == null) return;
        _dicePoint.text = _cc.DicePoint.GetPoint().ToString();
    }

}
