using EHTool.LangKit;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GUIUnitRoomPlayerState : MonoBehaviour
{

    [SerializeField] private Text _name;
    [SerializeField] private GUICharacterSettingUnit _characterSetting;
    [SerializeField] private Image _characterIcon;
    [SerializeField] private GameObject _readyIcon;
    [SerializeField] private Image _emoji;

    readonly float _waitingTime = 2f;

    private int _idx;

    public void SetInfor(string name, int idx, string characterName, bool isReady) {
        _name.text = name;
        _idx = idx;
        
        _characterSetting.SetData(new List<string>() { GameManager.Instance.GameSetting.Players[_idx].PlayerCharacter },
            GameManager.Instance.GameSetting.Players[_idx].PlayerCharacter, PlayerCharacterChange);

        _characterIcon.sprite =
            CharacterManager.Instance.GetCharacterData(characterName).Image;

        _readyIcon.SetActive(isReady);
        _emoji.gameObject.SetActive(false);
    }

    public void PlayerCharacterChange(string characterCode)
    {
        GameManager.Instance.GameSetting.SetPlayer(_idx, characterCode);
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
