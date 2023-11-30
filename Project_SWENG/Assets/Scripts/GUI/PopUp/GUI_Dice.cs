using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace UISystem {

    public class GUI_Dice : GUIPopUp {

        [SerializeField] private DicePoint _targetPlayer;
        [SerializeField] private Dice _dice;

        protected override void Open(Vector2 openPos)
        {
            base.Open(new Vector2(-800, 0));
            CamMovement.Instance.IsPlayerMove = true;
        }

        public void ReOpen() {
            gameObject.SetActive(true);
            CamMovement.Instance.IsPlayerMove = true;
            PopUpAction();
        }

        public void SetPlayer(DicePoint target)
        {
            _targetPlayer = target;
        }

        public void SetDiceValue()
        {
            _targetPlayer.SetPoint(_dice.Value);
            GameManager.Instance.gamePhase = GameManager.Phase.ActionPhase;
            Close();
        }

        public override void Close()
        {
            CamMovement.Instance.IsPlayerMove = false;
            gameObject.SetActive(false);
            transform.SetParent(UIManager.Instance.NowDisplay.transform.parent);
            UIManager.Instance.NowDisplay.PopPopUp();
        }

    }
}