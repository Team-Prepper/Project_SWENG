using UnityEngine;
using UnityEngine.UI;
using EasyH.Tool.LangKit;
using SWEng;

public class GUIUnitCharacterData : GUIUnitCharacterDataName {
    
    [SerializeField] private EHText _desc;

    [SerializeField] private Text _hp;
    [SerializeField] private Text _atk;
    [SerializeField] private Text _dfs;
    [SerializeField] private Text _levelTxt;

    private CharacterData _target;
    private int _level;

    protected override void Set(CharacterData data) {

        base.Set(data);

        _target = data;

        _desc.SetText(data.CharacterDesc);

        _level = 0;
        StatusShow(_level);

    }

    public void LevelChanged(int amount) {

        _level = Mathf.Clamp(_level + amount, 0,
            _target.StatusElements.Length - 1);

        StatusShow(_level);
    }

    private void StatusShow(int level) {

        CharacterData.StatusElement element =
            _target.StatusElements[level];

        _levelTxt.text = (level + 1).ToString();
        _hp.text = element.HP.ToString();
        _atk.text = element.Atk.ToString();
        _dfs.text = element.Dfs.ToString();

    }

}