using EHTool.LangKit;
using EHTool.UIKit;
using System.Collections.Generic;
using UnityEngine;

public class GUICharacterSelect : GUIPopUp {

    CallbackMethod<string> _callback;

    [SerializeField] GUICharacterSelectUnit[] _characters;
    [SerializeField] GameObject _listView;
    [SerializeField] EHText _message;

    public void Set(IList<string> except, CallbackMethod<string> callback)
    {

        _callback = callback;

        GameSetting gameSetting = new GameSetting();

        int i = 0;

        IList<string> characterList = gameSetting.AllCharacters;

        foreach (string str in except)
        {
            if (characterList.Contains(str))
                characterList.Remove(str);
        }

        if (characterList.Count == 0)
        {
            _message.gameObject.SetActive(true);
            _message.SetText("label_NoMoreCharacter");
            _listView.SetActive(false);
            return;
        }

        _message.gameObject.SetActive(false);
        _listView.SetActive(true);

        foreach (GUICharacterSelectUnit guiUnit in _characters)
        {
            if (i >= characterList.Count)
            {
                guiUnit.gameObject.SetActive(false);
                continue;
            }

            guiUnit.Set(gameSetting.AllCharacters[i++], ChangeTo);

        }
    }

    public void ChangeTo(string value)
    {
        _callback?.Invoke(value);
        Close();
    }

}