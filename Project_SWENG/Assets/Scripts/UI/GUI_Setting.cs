using UnityEngine;
using EasyH.LangKit;
using EasyH.Unity.UI;

public class GUI_Setting : GUIPopUp
{
    [System.Serializable]
    public class LangOption {
        public string key;
        public string value;
    }

    [SerializeField] private EHDropdownWrapper _langDropdown;
    [SerializeField] private LangOption[] _langOptions;

    public override void Open()
    {
        int idx = 0;

        string[] options = new string[_langOptions.Length];

        for (int i = 0; i < _langOptions.Length; i++) {
            if (LangManager.Instance.NowLang.Equals(_langOptions[i].value)) {
                idx = i;
            }
            options[i] = _langOptions[i].key;
        }

        _langDropdown.SetDropdownOption(options);
        _langDropdown.value = idx;

        _langDropdown.onValueChanged.AddListener(SetLanguage);
        
        base.Open();
    }

    public void SetLanguage(int idx)
    {
        LangManager.Instance.ChangeLang(_langOptions[idx].value);

    }

}
