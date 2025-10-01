using UnityEngine;
using Photon.Pun;
using System;
using System.Collections.Generic;
using EasyH;
using SWEng;

namespace MultiPlay.Photon.SWEng
{
    
    [RequireComponent(typeof(PhotonView))]
    public class PhotonCharacterStat :
        MonoBehaviourPun, ICharacterStat
    {
        [SerializeField] private PhotonView _pv;

        private ICharacter _targetCharacter;

        public string Name { get; }

        bool _isHuman = true;

        public int Level { get; set; } = 0;

        private string _characterCode;
        public string CharacterCode => _characterCode;

        private CharacterData.StatusElement[] _statusElement;

        private int _addedAtk = 0;
        private int _addedDfs = 0;

        public int Atk => _statusElement[Level].Atk + _addedAtk;
        public int Dfs => _statusElement[Level].Dfs + _addedDfs;

        private string _skill;
        public string Skill
        {
            get
            {
                return _skill;
            }
            private set
            {
                if (!_isHuman) return;

                _skill = value;
                SkillData skillData =
                    SkillDataManager.Instance.GetSkillData(_skill);
                _targetCharacter.Actor.ChangeAnimClip(
                    "Attack", skillData.AnimClip);
            }
        }

        private ISet<IObserver<ICharacterStat>> _observers
            = new HashSet<IObserver<ICharacterStat>>();

        public IDisposable Subscribe(IObserver<ICharacterStat> observer)
        {
            if (!_observers.Contains(observer))
            {
                _observers.Add(observer);

                observer.OnNext(this);
            }

            return new Unsubscriber<ICharacterStat>(_observers, observer);
        }

        private void Notify()
        {
            foreach (var obs in _observers)
            {
                obs.OnNext(this);
            }
        }

        private void Start()
        { 
            _pv = _pv != null ? _pv : GetComponent<PhotonView>();
        }

        public void SetCharacter(ICharacter character)
        {
            _targetCharacter = character;
        }

        public void SetCharacterCode(string characterCode)
        {
            _pv.RPC(nameof(PunSetCharacterCode),
                RpcTarget.All, characterCode);
        }

        [PunRPC]
        public void PunSetCharacterCode(string characterCode)
        {
            _characterCode = characterCode;

            CharacterData data = CharacterDataManager.Instance.
                GetCharacterData(_characterCode);

            _targetCharacter.Actor = Instantiate(
                data.Actor, _targetCharacter.transform);
            _targetCharacter.Status.Subscribe(
                _targetCharacter.Actor.GetHPBar());

            _statusElement = data.StatusElements;
            _isHuman = data.IsHumanType;
            _skill = data.DefaultSkill;

            _targetCharacter.Status.SetHP(_statusElement[0].HP,
                _statusElement[0].HP);

        }

        public void AddAtk(int amount)
        {
            _pv.RPC(nameof(PunSetAddedAtk),
                RpcTarget.All, _addedAtk + amount);
            _addedAtk += amount;
            Notify();
        }

        [PunRPC]
        private void PunSetAddedAtk(int value)
        {
            _addedAtk = value;
            Notify();
        }

        public void AddDfs(int amount)
        {
            _pv.RPC(nameof(PunSetAddedDfs),
                RpcTarget.All, _addedDfs + amount);
        }

        [PunRPC]
        private void PunSetAddedDfs(int value)
        {
            _addedDfs = value;
            Notify();
        }

    }
}