using EHTool.UIKit;
using System.Collections.Generic;
using UnityEngine;

public class GUICharacterSelect : GUIPopUp {

    CallbackMethod<string> _callback;

    [SerializeField] GUICharacterSelectUnit[] _characters;

    public void Set(IList<string> except, CallbackMethod<string> callback) {

        _callback = callback;

        GameSetting gameSetting = new GameSetting();

        int i = 0;

        IList<string> characterList = gameSetting.AllCharacters;

        foreach (string str in except) {
            if (characterList.Contains(str))
                characterList.Remove(str); 
        }

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

    public void ChangeTo(string value) {
        _callback?.Invoke(value);
        Close();
    }

}