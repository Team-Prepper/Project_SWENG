using System;

namespace SWEng.GamePlay
{
    public interface IStatus : IObservable<IStatus>
    {
        public int MaxHP { get; }
        public int CurHP { get; }

        public Action OnDeathEvent { get; set; }
        public Action OnDamageEvent { get; set; }

        public void SetHP(int curHP, int maxHP);
        public void Heal(int amount);
        public void TakeDamage(int amount);

    }
}