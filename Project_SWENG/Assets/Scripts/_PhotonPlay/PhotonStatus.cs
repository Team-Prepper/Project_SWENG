using UnityEngine;
using EHTool.UtilKit;
using Photon.Pun;
using System;
using System.Collections.Generic;
using EHTool;

public class PhotonStatus : MonoBehaviourPun, IStatus, IPunObservable
{

    public string Name { get; }

    bool _isHuman = true;
    private string _characterCode;
    public string CharacterCode {
        get {
            return _characterCode;
        }
        set {
            _characterCode = value;
            CharacterData data = CharacterManager.Instance.GetCharacterData(_characterCode);
            _statusElement = data.StatusElements;
            _skill = data.DefaultSkill;
            _isHuman = data.IsHumanType;
            
            HP = new GaugeInt(_statusElement[0].HP);
        } 
    }

    public GaugeValue<int> HP { get; private set; } = new GaugeInt(100, 100);
    public int Level { get; set; }

    private CharacterData.StatusElement[] _statusElement;

    private int _addedAtk = 0;
    private int _addedDfs = 0;
    public int Atk => _statusElement[Level].Atk + _addedAtk;
    public int Dfs => _statusElement[Level].Dfs + _addedDfs;

    private SkillData _skill;
    public SkillData Skill { 
        get {
            return _skill;
        }
        private set {
            if (!_isHuman) return;
            _skill = value;
            _cc.Character.ChangeAnimClip("Attack", _skill.AnimClip);
        }
    }

    public bool IsAlive => HP.Value > 0;

    private ICharacterController _cc;
    [SerializeField] private PhotonView _view;

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

    public void AddAtk(int amount)
    {
        _addedAtk += amount;
    }

    public void AddDfs(int amount)
    {
        _addedDfs += amount;
    }
    
    public void Heal(int amount) {

        HP.AddValue(amount);
        _view.RPC("SetHP", RpcTarget.All, HP.Value, HP.MaxValue);

    }

    public void TakeDamage(int amount)
    {
        if (!IsAlive) return;

        HP.SubValue(CalcDamage(amount));
        _view.RPC("SetHP", RpcTarget.All, HP.Value, HP.MaxValue);

        if (IsAlive)
        {
            _cc.PlayAnim("SetTrigger", "Hit");
            return;
        }

        _cc.PlayAnim("SetTrigger", "Die");
        _cc.Remove();

    }

    [PunRPC]
    public void SetHP(int cur, int max)
    {
        HP = new GaugeInt(cur, max);
        Notify();
    }

    public int CalcDamage(int damage)
    {
        return Mathf.Max(damage - Dfs, 1);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(_addedAtk);
            stream.SendNext(_addedDfs);

            Notify();
            return;
        }

        _addedAtk = (int)stream.ReceiveNext();
        _addedDfs = (int)stream.ReceiveNext();

        Notify();

    }

}