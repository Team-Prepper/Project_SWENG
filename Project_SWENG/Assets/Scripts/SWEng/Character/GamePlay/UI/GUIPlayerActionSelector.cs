using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using EasyH.Unity.UI;
using SWEng;

public class GUIPlayerActionSelect : GUICustomFullScreen, ICharacterController {

    [SerializeField] private BarHPBar _playerHealth;
    [SerializeField] private Text _dicePoint;

    [SerializeField] private RectTransform _panelBtnTr;
    [SerializeField] private float _openTime;

    [SerializeField] private Button _btnInteraction;
    [SerializeField] private Button _btnAttack;
    [SerializeField] private Button _btnMove;
    [SerializeField] private Button _btnInventory;
    [SerializeField] private Button _btnDice;

    private IDictionary<ICharacter, IList<ICharacterController.Action>> _todoList
        = new Dictionary<ICharacter, IList<ICharacterController.Action>>();
    private ICharacter _cc;

    private GUIDice _dicePopUp;

#nullable enable
    private IDisposable? _disposable;

    private void SetCharacterController(ICharacter cc)
    {
        _cc?.Actor.SetHPViewActive(true);

        _cc = cc;
        _cc.Actor.SetHPViewActive(false);
        _disposable?.Dispose();
        _disposable = _cc.Status.Subscribe(_playerHealth);
        
    }

    public void Ready(ICharacter cc, IList<ICharacterController.Action> actionList)
    {
        gameObject.SetActive(true);

        if (_cc == null) {
            SetCharacterController(cc);
        }
        if (_cc == cc) {
            ViewSet(actionList);
            return;
        }

        _todoList.Add(cc, actionList);

    }

    private void ViewSet(IList<ICharacterController.Action> actionList) {

        _cc.CameraController.CamSetting("Character");

        _btnInteraction.interactable = actionList.Contains
            (ICharacterController.Action.Interaction);
        _btnDice.interactable = actionList.Contains
            (ICharacterController.Action.Dice);
        _btnAttack.interactable = actionList.Contains
            (ICharacterController.Action.Attack);
        _btnMove.interactable = actionList.Contains
            (ICharacterController.Action.Move);
        _btnInventory.interactable = true;

        StartCoroutine(_PanelOpen());

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

    public void SelectAttackPoint(Action<IList<IDamageable>> attack) {

        UIManager.Instance.OpenGUI<GUIAttackSelect>
            ("AttackSelect").Set(attack, _cc);

    }

    public void OpenAttack()
    {
        SkillManager.Instance.GetSkill(
            _cc.Stat.Skill).Set(this, _cc);
        _AfterAction();
    }

    public void OpenMove()
    {
        UIManager.Instance.OpenGUI<GUIMoving>("Move")?.Set(_cc);
        _AfterAction();
    }

    public void OpenDice()
    {
        if(_dicePopUp == null) {
            _dicePopUp =
                UIManager.Instance.OpenGUI<GUIDice>("Dice");
        }
        else {
            _dicePopUp.ReOpen();
        }

        _dicePopUp.SetRollMethod((value) =>
            {
                _cc.DicePoint.SetPoint(value);
                _dicePopUp.Close();
            });
            
        _dicePopUp.AddCloseMethod(() => {
            _cc.ActionEnd(0);
            _cc.IsRollDice = true;
        });

        _AfterAction();
    }

    public void OpenInteraction()
    {
        UIManager.Instance.OpenGUI<GUIInteraction>
            ("Interaction").Set(_cc);
        _AfterAction();

    }

    public void OpenInventory() {
        UIManager.Instance.OpenGUI<GUIInventory>
            ("Inventory").Set(_cc);
        _AfterAction();
    }

    public void TurnEndButton()
    {
        if (_nowPopUp != null) return;
        
        _AfterAction();
        _cc.TurnMemberState.EndTurn();
        _cc = null;

        if (_todoList.Count < 1) {
            ItemManager.Instance.ShopItemInitial(5);
            return;
        }

        SetCharacterController(_todoList.ToList()[0].Key);
        ViewSet(_todoList[_cc]);
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
