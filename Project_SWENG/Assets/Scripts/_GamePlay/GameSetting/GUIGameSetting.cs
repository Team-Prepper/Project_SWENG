using EHTool.UIKit;
using System;
using System.Collections.Generic;
using UnityEngine;

public class GUIGameSetting : GUIPlayerSetting
{

    [SerializeField] GUIGameSettingUnit[] _enemyData;
    [SerializeField] GUIGameSettingUnit[] _bossEnemyData;

    protected override void Display() {

        base.Display();

        _Display(_enemyData, _gameSetting.Enemy, EnemyCharacterRemove);
        _Display(_bossEnemyData, _gameSetting.BossEnemy, BossEnemyCharacterRemove);

    }

    void _Display(GUIGameSettingUnit[] guiUnits, IList<string> value, Action<string> deleteAction)
    {
        int i = 0;

        foreach (GUIGameSettingUnit guiUnit in guiUnits)
        {
            if (i < value.Count)
            {
                guiUnit.SetData(this, value[i++], deleteAction);
                continue;
            }

            guiUnit.gameObject.SetActive(false);
        }

    }

    public void AddEnemy() {

        UIManager.Instance.OpenGUI<GUICharacterSelect>("CharacterSelect").Set(_gameSetting.Enemy, (value) =>
        {
            _gameSetting.AddEnemy(value);
            _Display(_enemyData, _gameSetting.Enemy, EnemyCharacterRemove);
        });
    }

    public void EnemyCharacterRemove(string characterCode)
    {
        if (_gameSetting.Enemy.Count < 2)
        {
            return;
        }
        _gameSetting.RemoveEnemy(characterCode);
        _Display(_enemyData, _gameSetting.Enemy, EnemyCharacterRemove);

    }
    public void AddBossEnemy()
    {

        UIManager.Instance.OpenGUI<GUICharacterSelect>("CharacterSelect").Set(_gameSetting.BossEnemy, (value) =>
        {
            _gameSetting.AddBossEnemy(value);
            _Display(_bossEnemyData, _gameSetting.BossEnemy, BossEnemyCharacterRemove);
        });
    }

    public void BossEnemyCharacterRemove(string characterCode)
    {
        if (_gameSetting.BossEnemy.Count < 2)
        {
            return;
        }
        _gameSetting.RemoveBossEnemy(characterCode);
        _Display(_bossEnemyData, _gameSetting.BossEnemy, BossEnemyCharacterRemove);

    }

}