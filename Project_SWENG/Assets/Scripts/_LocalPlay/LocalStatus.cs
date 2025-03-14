using UnityEngine;
using EHTool.UtilKit;
using EHTool;
using System;
using System.Collections.Generic;

public class LocalStatus : MonoBehaviour, IStatus {

    public string Name { get; }
    private string _characterCode;
    public string CharacterCode {
        get {
            return _characterCode;
        }
        set {
            _characterCode = value;
            CharacterData data = CharacterManager.Instance.GetCharacterData(_characterCode);
            _statusElement = data.StatusElements;
            Attack = data.DefaultSkill;
        } 
    }

    [SerializeField] private GaugeInt _hp = new GaugeInt(100, 100);
    public GaugeValue<int> HP => _hp;

    private CharacterData.StatusElement[] _statusElement;

    public int Level { get; set; }

    private int _addedAtk = 0;
    private int _addedDfs = 0;
    public int Atk => _statusElement[Level].Atk + _addedAtk;
    public int Dfs => _statusElement[Level].Dfs + _addedDfs;

    public string Attack { get; private set; } = "BasicTargetingSkill";
    public bool IsAlive => HP.Value > 0;

    private ICharacterController _cc;
    private ISet<IObserver<IStatus>> _observers
        = new HashSet<IObserver<IStatus>>();

    public IDisposable Subscribe(IObserver<IStatus> observer)
    {
        if (!_observers.Contains(observer))
        {
            _observers.Add(observer);

            observer.OnNext(this);
        }

        return new Unsubscriber<IStatus>(_observers, observer);
    }

    private void Notify()
    {
        foreach (var obs in _observers)
        {
            obs.OnNext(this);
        }
    }

    public void SetCC(ICharacterController cc)
    {
        _cc = cc;
    }

    public void AddAtk(int amount) {
        _addedAtk += amount;
        Notify();
    }

    public void AddDfs(int amount) {
        _addedDfs += amount;
        Notify();
    }

    public void TakeDamage(int amount) {

        if (!IsAlive) return;

        HP.SubValue(CalcDamage(amount));
        Notify();
        
        if (IsAlive)
        {
            _cc.PlayAnim("SetTrigger", "Hit");
            return;
        }

        _cc.PlayAnim("SetTrigger", "Die");
        _cc.Remove();

    }

    public int CalcDamage(int damage) {
        return Mathf.Max(damage - Dfs, 1);
    }

}