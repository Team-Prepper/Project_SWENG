using EHTool.UIKit;
using System;
using System.Collections.Generic;
using UnityEngine;

public class GUIPlayerSetting : GUIPopUp {

    protected IGameSetting _gameSetting;

    [SerializeField] GUICharacterSettingUnit _playerData;

    Action _callback;

    public override void Open()
    {

        base.Open();

        if (GameManager.Instance.GameSetting != null)
        {
            _gameSetting = GameManager.Instance.GameSetting;
        }
        else
        {
            _gameSetting = new LocalGameSetting();
        }

        Display();

    }

    public void SetCloseCallback(Action callback)
    {
        _callback = callback;
    }

    protected virtual void Display()
    {
        _playerData.SetData(new List<string>() { _gameSetting.Players[0].PlayerCharacter },
            _gameSetting.Players[0].PlayerCharacter, PlayerCharacterChange);

    }

    public void PlayerCharacterChange(string characterCode)
    {

        _gameSetting.SetPlayer(0, characterCode);

        Display();
    }

    public void Apply()
    {
        _callback?.Invoke();
        GameManager.Instance.GameSetting = _gameSetting;
        Close();

    }

}
