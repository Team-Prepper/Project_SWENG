using EHTool.LangKit;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIUnitRoomPlayerState : MonoBehaviour
{

    [SerializeField] Text _name;
    [SerializeField] EHText _characterName;
    [SerializeField] GameObject _readyIcon;
    [SerializeField] Image _emoji;

    readonly float _waitingTime = 2f;

    public void SetInfor(string name, string characterName, bool isReady) {
        _characterName.SetText(characterName);
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
