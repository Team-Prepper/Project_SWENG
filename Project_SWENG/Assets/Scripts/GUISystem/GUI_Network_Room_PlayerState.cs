using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUI_Network_Room_PlayerState : MonoBehaviour
{

    [SerializeField] Text _name;
    [SerializeField] GameObject _readyIcon;
    [SerializeField] Image _emoji;

    readonly float _waitingTime = 2f;

    public void SetInfor(string name, bool isReady) {
        _name.text = name;
        _readyIcon.SetActive(isReady);
        _emoji.gameObject.SetActive(false);
    }

    public void DisplayEmoji(Sprite spr) { 
        _emoji.sprite = spr;
        _emoji.gameObject.SetActive(true);

        StartCoroutine(Waiting());
    }

    IEnumerator Waiting() {
        yield return new WaitForSeconds(_waitingTime);
        _emoji.gameObject.SetActive(false);

    }
}
