using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace UISystem {

    public class GUI_Dice : GUIPopUp {

        [SerializeField] private IDicePoint _targetPlayer;
        [SerializeField] private Dice _dice;

        public void ReOpen() {
            gameObject.SetActive(true);
            PopUpAction();
        }

        public void SetPlayer(IDicePoint target)
        {
            _targetPlayer = target;
        }

        public void SetDiceValue()
        {
            _targetPlayer.SetPoint(_dice.Value);
            Close();
        }

        public override void Close()
        {
            gameObject.SetActive(false);
            UIManager.Instance.NowDisplay.PopPopUp();
        }

    }
}