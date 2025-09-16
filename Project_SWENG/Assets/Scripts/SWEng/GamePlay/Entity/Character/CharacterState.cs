using UnityEngine;
using SWEng.Data;
using System;
using System.Collections.Generic;
using EasyH;

namespace SWEng.GamePlay
{
    public class CharacterStat : MonoBehaviour, ICharacterStat
    {
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

        public void SetCharacter(ICharacter character)
        {
            _targetCharacter = character;
        }

        public void SetCharacterCode(string characterCode)
        {
            _characterCode = characterCode;
            CharacterData data = CharacterDataManager.Instance.GetCharacterData(_characterCode);

            _isHuman = data.IsHumanType;
            _statusElement = data.StatusElements;
            _skill = data.DefaultSkill;

            _targetCharacter.Status.SetHP(_statusElement[0].HP,
                _statusElement[0].HP);
            _targetCharacter.Actor = Instantiate(data.Actor, _targetCharacter.transform);
            _targetCharacter.Actor.transform.localPosition = Vector3.zero;

        }

        public void AddAtk(int amount)
        {
            _addedAtk += amount;
            Notify();
        }

        public void AddDfs(int amount)
        {
            _addedDfs += amount;
            Notify();
        }

    }
}