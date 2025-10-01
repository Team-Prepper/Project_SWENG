using UnityEngine;
using EasyH;
using System;
using System.Collections.Generic;

namespace SWEng {
    
    [RequireComponent(typeof(DamageCalcer))]
    public class Status : MonoBehaviour, IStatus
    {
        public Action OnDamageEvent { get; set; }
        public Action OnDeathEvent { get; set; }

        public int MaxHP { get; private set; }
        public int CurHP { get; private set; }

        private ISet<IObserver<IStatus>> _observers
            = new HashSet<IObserver<IStatus>>();

        [SerializeField] private DamageCalcer _damageCalcer;

        private void Start()
        {
            _damageCalcer = _damageCalcer != null ?
                _damageCalcer : GetComponent<DamageCalcer>();
        }

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

        public void SetHP(int curHP, int maxHP)
        {
            CurHP = curHP;
            MaxHP = maxHP;
            Notify();
        }

        public void Heal(int amount)
        {
            CurHP = Mathf.Clamp(CurHP + amount, 0, MaxHP);
            Notify();
        }

        public void TakeDamage(int amount)
        {
            if (CurHP <= 0) return;

            CurHP = Mathf.Clamp(
                CurHP - _damageCalcer.CalcDamage(amount),
                0, MaxHP);

            Notify();

            if (CurHP > 0)
            {
                OnDamageEvent?.Invoke();
                return;
            }

            OnDeathEvent?.Invoke();

        }

    }
}