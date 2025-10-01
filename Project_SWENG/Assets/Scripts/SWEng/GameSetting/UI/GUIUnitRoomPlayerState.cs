using EasyH.Tool.LangKit;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using SWEng;

public class GUIUnitRoomPlayerState : MonoBehaviour
{

    [SerializeField] private Text _name;
    [SerializeField] private GUIUnitCharacterSetting _characterSetting;
    [SerializeField] private Image _characterIcon;
    [SerializeField] private GameObject _readyIcon;
    [SerializeField] private Image _emoji;

    private readonly float _waitingTime = 2f;

    private int _idx;

    public void SetInfor(int idx, string name, string characterCode, bool isReady) {
        _idx = idx;
        
        _characterSetting.SetData(new List<string>() { GameManager.Instance.Setting.Players[_idx].PlayerCharacter },
            GameManager.Instance.Setting.Players[_idx].PlayerCharacter, PlayerCharacterChange);

        CharacterData data = CharacterDataManager.Instance.GetCharacterData(characterCode);
        _characterIcon.sprite = data.Image;
        _name.text = name;

        _readyIcon.SetActive(isReady);
        _emoji.gameObject.SetActive(false);
    }

    public void PlayerCharacterChange(string characterCode)
    {
        GameManager.Instance.Setting.SetPlayer(_idx, characterCode);
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
