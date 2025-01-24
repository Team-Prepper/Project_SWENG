using EHTool.UIKit;
using System;
using System.Collections.Generic;
using UnityEngine;

public class GUIPlayerSetting : GUIPopUp {

    protected GameSetting _gameSetting;

    [SerializeField] GUICharacterSettingUnit _playerData;

    Action _callback;

    public override void Open()
    {

        base.Open();

        if (GameManager.Instance.GameMaster.Setting != null)
        {
            _gameSetting = GameManager.Instance.GameMaster.Setting;
        }
        else
        {
            _gameSetting = new GameSetting();
        }

        Display();

    }

    public void SetCloseCallback(Action callback)
    {
        _callback = callback;
    }

    protected virtual void Display()
    {
        _playerData.SetData(new List<string>() { _gameSetting.Player },
            _gameSetting.Player, PlayerCharacterChange);

    }

    public void PlayerCharacterChange(string characterCode)
    {

        _gameSetting.Player = characterCode;

        Display();
    }

    public void Apply()
    {
        _callback?.Invoke();
        GameManager.Instance.GameMaster.Setting = _gameSetting;
        Close();

    }

}
