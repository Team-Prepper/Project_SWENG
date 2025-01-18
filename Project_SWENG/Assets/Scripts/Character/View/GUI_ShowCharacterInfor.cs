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

    ICharacterController _client;


    public void SetInfor(ICharacterController target, ICharacterController client)
    {
        CharacterStatus character = target.Character.Status;
        CameraManager.Instance.CameraSetting(target.transform, "Character");
        _client = client;

        _name.SetText(character.GetName());
        _lv.text = character.Level.ToString();
        _hp.text = character.HP.ToString();
        _attackValue.text = character.AttackValue.ToString();

    }

    public override void Close()
    {
        base.Close();
        _client.ActionEnd(0);
    }
}