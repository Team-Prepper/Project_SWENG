using UnityEngine;
using EHTool.LangKit;
using EHTool.UIKit;
using UnityEngine.UI;

public class GUI_ShowCharacterInfor : GUIPopUp
{
    [SerializeField] protected EHText _name;
    [SerializeField] protected Text _lv;
    [SerializeField] protected Text _hp;
    [SerializeField] protected Text _attackValue;


    public void SetInfor(string name, CharacterSystem.Character target) {
        _name.SetText(name);
        _lv.text = target.stat.GetLevel().ToString();
        _hp.text = target.stat.HP.ToString();
        _attackValue.text = target.GetAttackValue().ToString();

    }
}
