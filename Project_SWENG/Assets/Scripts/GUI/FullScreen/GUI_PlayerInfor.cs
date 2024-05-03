using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using CharacterSystem;
using UISystem;
using Photon.Pun;
using Unity.VisualScripting;

public class GUI_PlayerInfor : GUIFullScreen, IActionSelector {

    [SerializeField] private GameObject _target;
    [SerializeField] private GameObject _turnEndGlowLight;
    [SerializeField] private GUI_PlayerHealth _playerHealth;
    [SerializeField] private TextMeshProUGUI _dicePoint;

    [SerializeField] GameObject panelBtn;
    [SerializeField] Button btnAttack;
    [SerializeField] Button btnSkill;
    [SerializeField] Button btnMove;
    [SerializeField] Button btnDice;

    public Button _turnEndButton;

    ICharacterController _targetCC;
    PlayerCharacter _targetPlayer;

    protected override void Open(Vector2 openPos)
    {
        base.Open(openPos);
    }

    public void SetPlayer(GameObject target)
    {

        _target = target;
        CamMovement.Instance.SetCamTarget(target);
        _targetPlayer = target.GetComponent<PlayerCharacter>();

        _targetPlayer.SetHealthUI(_playerHealth);

    }

    public void SetCharacterController(ICharacterController cc)
    {
        _targetCC = cc;

    }

    public void Ready(IList<Character.Action> actionList)
    {
        panelBtn.SetActive(true);

        CamMovement.Instance.ConvertCharacterCam();
        btnDice.interactable = actionList.Contains(Character.Action.Dice);
        btnAttack.interactable = actionList.Contains(Character.Action.Attack);
        btnMove.interactable = actionList.Contains(Character.Action.Move);

    }

    public void OpenAttack()
    {
        UIManager.OpenGUI<GUI_Attack>("Attack").Set(_targetPlayer);
        _AfterAction();
    }

    public void OpenDice()
    {
        UIManager.Instance.UseDice(_targetPlayer);
        _AfterAction();
    }

    public void OpenMove()
    {
        UIManager.OpenGUI<GUI_Moving>("Move").Set(_targetPlayer);
        _AfterAction();
    }

    public void TurnEndButton()
    {
        if (_nowPopUp) return;
        _AfterAction();
        _targetCC.TurnEnd();
    }

    void _AfterAction() {

        panelBtn.SetActive(false);
    }

    protected override void Update()
    {
        base.Update();
        _dicePoint.text = _targetPlayer.GetPoint().ToString();
        _turnEndGlowLight.SetActive(_turnEndButton.interactable);
    }


    /*
    public void AttackButton() {
        if (_nowPopUp) return;
        if (!_targetPlayer.CanAttack()) return;
    }

    public void OpenInforPopUp() {
        if (_nowPopUp) return;
        UIManager.OpenGUI<GUI_ShowCharacterInfor>("PlayerInforPopUp").SetInfor(PhotonNetwork.NickName, _targetPlayer);
    }
    */

}
