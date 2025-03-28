using EHTool.UIKit;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIGameSetting : GUIPopUp
{

    [SerializeField] private GameSettingData[] _presets;
    [SerializeField] private EHDropdownWrapper _presetDropdown;
    [SerializeField] private Text _enemyCntText;
    [SerializeField] private Text _phaseCntText;

    private string _mapName = "Local";
    private int _enemyCnt = 1;
    private int _phaseCnt = 1;

    private IList<string> _enemyList;
    private IList<string> _bossEnemyList;

    public override void Open()
    {
        base.Open();

        _enemyList = new List<string>(GameManager.Instance.GameSetting.Enemy);
        _bossEnemyList = new List<string>(GameManager.Instance.GameSetting.BossEnemy);

        string[] options = new string[_presets.Length];
        for (int i = 0; i < _presets.Length; i++) {
            options[i] = _presets[i].Name;
        }

        _presetDropdown.SetDropdownOption(options);
        _presetDropdown.onValueChanged.AddListener(SetPreset);

        Display();
    }

    public void OpenEnemySetting() {
        UIManager.Instance.OpenGUI<GUIEnemySetting>("EnemySetting")
            .SetList(_enemyList, _bossEnemyList);
    }

    public void SetPreset(int idx) {
        
        _mapName = _presets[idx].MapName;
        
        _phaseCnt = _presets[idx].PhaseCnt;
        _enemyCnt = _presets[idx].PhaseEnemyCnt;
        
        _enemyList = _presets[idx].EnemyList;
        _bossEnemyList = _presets[idx].BossEnemyList;

        Display();
    }

    public void EnemyCntChange(int amount) {
        _enemyCnt += amount;
        _enemyCnt = Mathf.Max(1, _enemyCnt);
        Display();
    }
    
    public void PhaseCntChange(int amount) {
        _phaseCnt += amount;
        _phaseCnt = Mathf.Max(1, _phaseCnt);
        Display();
    }

    private void Display() {
        _phaseCntText.text = _phaseCnt.ToString();
        _enemyCntText.text = _enemyCnt.ToString();
    }

    public void Apply() {

        GameManager.Instance.GameSetting.Enemy = _enemyList;
        GameManager.Instance.GameSetting.BossEnemy = _bossEnemyList;

        GameManager.Instance.GameSetting.PhaseCnt = _phaseCnt;
        GameManager.Instance.GameSetting.EnemyCnt = _enemyCnt;

        GameManager.Instance.GameSetting.MapName = _mapName;

        Close();
    }

}