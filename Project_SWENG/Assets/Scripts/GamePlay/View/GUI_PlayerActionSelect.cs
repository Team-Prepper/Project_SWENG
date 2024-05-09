using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CharacterSystem;
using UISystem;
using Photon.Realtime;

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

    ICharacterController _targetCC;
    PlayerCharacter _targetPlayer;

    public void SetPlayer(GameObject target)
    {
        
        CamMovement.Instance.SetCamTarget(target.transform);
        _targetPlayer = target.GetComponentInChildren<PlayerCharacter>();

        _targetPlayer.SetHealthUI(_playerHealth);

    }

    public void SetCharacterController(ICharacterController cc)
    {
        _targetCC = cc;

    }

    public void Ready(IList<Character.Action> actionList)
    {
        _targetCC.CamSetting();

        btnDice.interactable = actionList.Contains(Character.Action.Dice);
        btnAttack.interactable = actionList.Contains(Character.Action.Attack);
        btnMove.interactable = actionList.Contains(Character.Action.Move);

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
        _targetCC.DoAttack();
        _AfterAction();
    }

    public void OpenMove()
    {
        _targetCC.DoMove();
        _AfterAction();
    }

    public void OpenDice()
    {
        UIManager.Instance.UseDice(_targetPlayer);
        _AfterAction();
    }

    public void TurnEndButton()
    {
        if (_nowPopUp != null) return;
        _AfterAction();
        _targetCC.TurnEnd();
    }

    void _AfterAction() {

        panelBtnTr.gameObject.SetActive(false);
    }

    protected override void Update()
    {
        base.Update();
        _dicePoint.text = _targetPlayer.GetPoint().ToString();
        _turnEndGlowLight.SetActive(_turnEndButton.interactable);
    }

}
