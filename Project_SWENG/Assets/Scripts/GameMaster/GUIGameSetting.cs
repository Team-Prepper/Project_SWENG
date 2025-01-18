using EHTool.UIKit;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIGameSetting : GUIPlayerSetting
{

    [SerializeField] GUIGameSettingUnit[] _enemyData;
    [SerializeField] GUIGameSettingUnit[] _bossEnemyData;

    protected override void Display() {

        base.Display();

        _Display(_enemyData, _gameSetting.Enemy);
        _Display(_bossEnemyData, _gameSetting.BossEnemy);

    }

    void _Display(GUIGameSettingUnit[] guiUnits, IList<string> value)
    {
        int i = 0;

        foreach (GUIGameSettingUnit guiUnit in guiUnits)
        {
            if (i < value.Count)
            {
                guiUnit.SetData(this, value[i++]);
                continue;
            }

            guiUnit.gameObject.SetActive(false);
        }

    }

    public void AddEnemy() {

        UIManager.Instance.OpenGUI<GUICharacterSelect>("CharacterSelect").Set(_gameSetting.Enemy, (value) =>
        {
            _gameSetting.Enemy.Add(value);
            _Display(_enemyData, _gameSetting.Enemy);
        });
    }

    public void EnemyCharacterRemove(string characterCode)
    {
        if (_gameSetting.Enemy.Count < 2)
        {
            return;
        }
        _gameSetting.Enemy.Remove(characterCode);
        _Display(_enemyData, _gameSetting.Enemy);

    }

    public void BossEnemyCharacterRemove(string characterCode)
    {
        if (_gameSetting.BossEnemy.Count < 2)
        {
            return;
        }
        _gameSetting.BossEnemy.Remove(characterCode);
        _Display(_bossEnemyData, _gameSetting.BossEnemy);

    }

}