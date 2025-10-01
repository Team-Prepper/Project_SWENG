using UnityEngine;
using Photon.Pun;
using EasyH;
using System;
using System.Collections.Generic;
using SWEng;

namespace MultiPlay.Photon.SWEng
{
    
    [RequireComponent(typeof(PhotonView))]
    public class PhotonStatus : MonoBehaviourPun, IStatus
    {
        public Action OnDamageEvent { get; set; }
        public Action OnDeathEvent { get; set; }

        public int MaxHP { get; private set; }
        public int CurHP { get; private set; }

        public bool IsAlive => CurHP > 0;

        [SerializeField] private PhotonView _pv;
        [SerializeField] private DamageCalcer _damageCalcer;

        private ISet<IObserver<IStatus>> _observers
            = new HashSet<IObserver<IStatus>>();

        void Start()
        {
            _pv = _pv != null ? _pv : GetComponent<PhotonView>();
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
            _pv.RPC(nameof(PunSetHP), RpcTarget.All, curHP, maxHP);
        }

        [PunRPC]
        private void PunSetHP(int curHP, int maxHP)
        { 
            CurHP = curHP;
            MaxHP = maxHP;

            Notify();
        }

        public void Heal(int amount)
        {
            _pv.RPC(nameof(PunHeal), RpcTarget.All, amount);
        }

        [PunRPC]
        public void PunHeal(int amount)
        {
            CurHP = Mathf.Clamp(CurHP + amount, 0, MaxHP);
            Notify();
        }

        public void TakeDamage(int amount)
        {
            _pv.RPC(nameof(PunTakeDamage), RpcTarget.All,
                _damageCalcer.CalcDamage(amount));
        }

        [PunRPC]
        public void PunTakeDamage(int amount)
        {
            if (CurHP <= 0) return;

            CurHP = Mathf.Clamp(CurHP - amount, 0, MaxHP);

            if (!PhotonNetwork.IsMasterClient) return;

            if (CurHP > 0)
            {
                OnDamageEvent?.Invoke();
                return;
            }

            OnDeathEvent?.Invoke();

        }

    }
}