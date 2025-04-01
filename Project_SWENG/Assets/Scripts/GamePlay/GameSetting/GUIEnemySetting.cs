using EHTool.UIKit;
using UnityEngine;
using System.Collections.Generic;
using System;

public class GUIEnemySetting : GUIPopUp
{
    [SerializeField] private List<GUIUnitCharacterSelect> _enemySelectUI;
    [SerializeField] private List<GUIUnitCharacterSelect> _bossSelectUI;

    private List<int> _enemyRemoveList;
    private List<int> _bossRemoveList;
    
    private IList<string> _enemyList;
    private IList<string> _bossList;

    public void SetList(IList<string> enemyList, IList<string> bossEnemyList) {

        _enemyList = enemyList;
        _bossList = bossEnemyList;

        _enemyRemoveList = new List<int>();
        _bossRemoveList = new List<int>();

        _Display(_enemySelectUI, _enemyList, AddEnemyRemoveList);
        _Display(_bossSelectUI, _bossList, AddBossRemoveList);

    }

    void _Display(List<GUIUnitCharacterSelect> guiUnits, IList<string> value, Action<int> deleteAction)
    {

        while (guiUnits.Count < value.Count) {
            Instantiate(guiUnits[0], guiUnits[0].transform.parent);
        } 

        for (int i = 0; i < guiUnits.Count; i++)
        {
            guiUnits[i].Set(value, i, deleteAction);
        }

    }

    public void AddEnemy() {

        UIManager.Instance.OpenGUI<GUICharacterSelect>("CharacterSelect").
            Set(_enemyList, (value) =>
                {
                    _enemyList.Add(value);
                    _Display(_enemySelectUI, _enemyList, AddEnemyRemoveList);
                }
            );
    }

    public void RemoveEnemy() {

        _enemyRemoveList.Sort((a, b) => { return b.CompareTo(a); });

        for (int i = 0; i < _enemyRemoveList.Count; i++) {
            _enemyList.RemoveAt(_enemyRemoveList[i]);
        }

        _enemyRemoveList = new List<int>();
        _Display(_enemySelectUI, _enemyList, AddEnemyRemoveList);

    }

    public void AddEnemyRemoveList(int idx)
    {
        if (_enemyRemoveList.Contains(idx)) {
            _enemyRemoveList.Remove(idx);
            _enemySelectUI[idx].SetLightActive(false);
            return;
        }

        if (_enemyList.Count - _enemyRemoveList.Count  < 2)
        {
            return;
        }

        _enemyRemoveList.Add(idx);
        _enemySelectUI[idx].SetLightActive(true);

    }

    public void AddBossEnemy()
    {

        UIManager.Instance.OpenGUI<GUICharacterSelect>("CharacterSelect").
            Set(_bossList, (value) =>
                {
                    _bossList.Add(value);
                    _Display(_bossSelectUI, _bossList, AddBossRemoveList);
                }
            );

    }
    public void RemoveBossEnemy() {
        _bossRemoveList.Sort((a, b) => { return b.CompareTo(a); });

        for (int i = 0; i < _bossRemoveList.Count; i++) {
            _bossList.RemoveAt(_bossRemoveList[i]);
        }

        _bossRemoveList = new List<int>();
        _Display(_bossSelectUI, _bossList, AddBossRemoveList);
    }

    public void AddBossRemoveList(int idx)
    {
        if (_bossRemoveList.Contains(idx)) {
            _bossRemoveList.Remove(idx);
            _bossSelectUI[idx].SetLightActive(false);
            return;
        }

        if (_bossList.Count - _bossRemoveList.Count < 2)
        {
            return;
        }

        _bossRemoveList.Add(idx);
        _bossSelectUI[idx].SetLightActive(true);

    }

}
