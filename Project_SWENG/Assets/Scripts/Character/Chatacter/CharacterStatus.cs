using UnityEngine;
using EHTool.UtilKit;

public class CharacterStatus : MonoBehaviour, ICharacterComponent {

    protected ICharacterController _cc;

    [SerializeField] private IHealthUI _healthUI;
    [SerializeField] private string _nameCode;
    [SerializeField] private Animator _anim;

    [SerializeField] private GaugeInt _hp;
    [SerializeField] private int _level = 1;
    //[SerializeField] private int _exp = 0;
    [SerializeField] private int _def = 0;
    [SerializeField] private int _attackPower = 10;

    public int Level => _level;
    public GaugeInt HP => _hp;
    public int AttackValue => _attackPower;
    public bool IsAlive => _hp.Value > 0;

    private void Start()
    {
        _healthUI?.UpdateGUI(_hp);
    }

    public void PlayAnim(string triggerType, string triggerValue)
    {
        switch (triggerType)
        {
            case "SetBoolTrue":
                _anim.SetBool(triggerValue, true);
                return;
            case "SetBoolFalse":
                _anim.SetBool(triggerValue, false);
                return;
            case "SetTrigger":
            default:
                _anim.SetTrigger(triggerValue);
                return;
        }
    }

    public void SetHealthUI(IHealthUI ui)
    {
        if (_healthUI != null)
        {
            Destroy(_healthUI);
        }

        _healthUI = ui;
        _healthUI.UpdateGUI(_hp);
    }

    public void SetCC(ICharacterController cc)
    {
        _cc = cc;
    }

    public void ChangeStat(string statName, int amount) {
        switch (statName) {
            case "HP":
                _hp.AddValue(amount);
                return;
            case "ATK":
                _attackPower += amount;
                return;
            default:
                return;
        }
    }

    public void TakeDamage(int amount)
    {
        if (!IsAlive) return;

        _hp.SubValue(Mathf.Max(CalcDamage(amount), 1));

        if (_healthUI != null)
        {
            _healthUI.UpdateGUI(_hp);
        }

    }

    public int CalcDamage(int damage)
    {
        return damage - _def;
    }

    public void Revive()
    {
        _hp.AddValue(1000);
    }

    public virtual string GetName()
    {
        return _nameCode;
    }

}