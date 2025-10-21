using UnityEngine;
using UnityEngine.UI;
using CameraSystem;
using SWEng;
using EasyH.Unity.UI;
using EasyH.Unity.LangKit;

public class GUI_ShowCharacterInfor : GUIPopUp
{
    [SerializeField] protected EHText _name;
    [SerializeField] protected Text _lv;
    [SerializeField] protected Text _hp;
    [SerializeField] protected Text _attackValue;

    private ICharacter _client;

    public void SetInfor(ICharacter target, ICharacter client)
    {
        _client = client;

        CameraManager.Instance.CameraSetting(
            target.transform, "Character");
        IStatus status = target.Status;
        ICharacterStat state = target.Stat;

        _name.SetText(state.CharacterCode);
        _lv.text = state.Level.ToString();
        _hp.text = string.Format("{0} / {1}",
            status.CurHP, status.MaxHP);
        _attackValue.text = state.Atk.ToString();

    }

    public override void Close()
    {
        base.Close();
        _client.ActionEnd(0);
    }
}