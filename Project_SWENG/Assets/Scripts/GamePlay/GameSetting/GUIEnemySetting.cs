using EHTool.UIKit;
using UnityEngine;
using System.Collections.Generic;
using System;

public class GUIEnemySetting : GUIPopUp
{
    [SerializeField] private List<GUIGameSettingUnit> _enemyData;
    [SerializeField] private List<GUIGameSettingUnit> _bossEnemyData;
    

    private IList<string> _enemyList;
    private IList<string> _bossEnemyList;

    public void SetList(IList<string> enemyList, IList<string> bossEnemyList) {
        _enemyList = enemyList;
        _bossEnemyList = bossEnemyList;

        Display();
    }

    private void Display() {

        _Display(_enemyData, _enemyList, EnemyCharacterRemove);
        _Display(_bossEnemyData, _bossEnemyList, BossEnemyCharacterRemove);

    }

    void _Display(List<GUIGameSettingUnit> guiUnits, IList<string> value, Action<string> deleteAction)
    {
        while (guiUnits.Count < value.Count) {
            Instantiate(guiUnits[0], guiUnits[0].transform.parent);
        } 

        int i = 0;
        foreach (GUIGameSettingUnit guiUnit in guiUnits)
        {
            if (i < value.Count)
            {
                guiUnit.SetData(value[i++], deleteAction);
                continue;
            }

            guiUnit.gameObject.SetActive(false);
        }

    }

    public void AddEnemy() {

        UIManager.Instance.OpenGUI<GUICharacterSelect>("CharacterSelect").
            Set(_enemyList, (value) =>
                {
                    _enemyList.Add(value);
                    _Display(_enemyData, _enemyList, EnemyCharacterRemove);
                }
            );
    }

    public void EnemyCharacterRemove(string characterCode)
    {
        if (_enemyList.Count < 2)
        {
            return;
        }
        _enemyList.Remove(characterCode);
        _Display(_enemyData, _enemyList, EnemyCharacterRemove);

    }

    public void AddBossEnemy()
    {

        UIManager.Instance.OpenGUI<GUICharacterSelect>("CharacterSelect").
            Set(_bossEnemyList, (value) =>
                {
                    _bossEnemyList.Add(value);
                    _Display(_bossEnemyData, _bossEnemyList, BossEnemyCharacterRemove);
                }
            );
    }

    public void BossEnemyCharacterRemove(string characterCode)
    {
        if (_bossEnemyList.Count < 2)
        {
            return;
        }
        _bossEnemyList.Remove(characterCode);
        _Display(_bossEnemyData, _bossEnemyList, BossEnemyCharacterRemove);

    }

}
