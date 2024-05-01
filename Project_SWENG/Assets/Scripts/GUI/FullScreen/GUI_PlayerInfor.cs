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
        _targetCC = target.GetComponent<ICharacterController>();
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
        _targetCC.TurnEnd();
        _AfterAction();
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

    public override void HexSelect(HexCoordinate selectGridPos)
    {
        /*
        if (GameManager.Instance.GameMaster.gamePhase == IGameMaster.Phase.EnemyPhase) {
            return;
        }*/

        return;

        Hex selected = HexGrid.Instance.GetTileAt(selectGridPos);

        if (!selected || !selected.Entity) return;

        if (selected.Entity == _target)
        {
            //UIManager.OpenGUI<GUI_ActionSelect>("ActionSelect").Set(_target);
            return;
        }

        if (!selected.Entity.TryGetComponent<Character>(out Character target)) return;

        UIManager.OpenGUI<GUI_ShowCharacterInfor>("CharacterInfor").SetInfor(target.GetName(), target);
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
