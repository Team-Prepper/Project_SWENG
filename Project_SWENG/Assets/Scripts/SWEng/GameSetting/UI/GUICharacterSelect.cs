using System;
using System.Collections.Generic;
using UnityEngine;
using SWEng;
using EasyH.Unity.UI;
using EasyH.Tool.LangKit;

public class GUICharacterSelect : GUIPopUp
{

    private Action<string> _callback;

    private IList<string> _characterList;

    [SerializeField] private GUIUnitCharacterSelect[] _characters;
    [SerializeField] private GUIUnitCharacterDataBase _selectedCharacterData;
    [SerializeField] private GameObject _listView;
    [SerializeField] private EHText _message;

    private int _idx;

    public void Set(IList<string> except, Action<string> callback)
    {

        _callback = callback;

        _characterList = CharacterDataManager.Instance.AllCharacters;

        foreach (string str in except)
        {
            if (_characterList.Contains(str))
                _characterList.Remove(str);
        }

        if (_characterList.Count == 0)
        {
            _message.gameObject.SetActive(true);
            _message.SetText("label_NoMoreCharacter");
            _listView.SetActive(false);
            return;
        }

        _message.gameObject.SetActive(false);
        _listView.SetActive(true);

        for (int i = 0; i < _characters.Length; i++)
        {
            _characters[i].Set(_characterList, i, ChangeTo);
        }

        _characters[0].Select();

    }

    public void ChangeTo(int value)
    {
        if (_idx >= 0) {
            _characters[_idx].SetLightActive(false);
        }
        
        _idx = value;
        _characters[_idx].SetLightActive(true);
        _selectedCharacterData.Set(_characterList[_idx]);

    }

    public void Select() {
        _callback?.Invoke(_characterList[_idx]);
        Close();

    }

}