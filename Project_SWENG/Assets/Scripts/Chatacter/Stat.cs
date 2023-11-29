using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character {
    [System.Serializable]
    public class Stat {

        [SerializeField] private int _lv = 1;
        [SerializeField] private int _exp = 0;
        [SerializeField] private GaugeValue<int> _hp = new GaugeValue<int>(100, 100, 0);
        [SerializeField] private int _def = 0;
        [SerializeField] private int _attackPower = 10;

        public GaugeValue<int> HP {
            get {
                return _hp;
            }
        }

        public bool IsAlive()
        {
            return _hp.Value != 0;
        }

        public int GetLevel() {
            return _lv;
        }

        public void SetDef(int value, bool add)
        {
            if(add)
                _def += value;
            else
                _def -= value;
        }
        
        public void SetAttackPower(int value, bool add)
        {
            if (add)
                _attackPower += value;
            else
                _attackPower -= value;
        }

        public bool GetExp(int val)
        {
            _exp += val;
            if(_exp >= 10 * _lv)
            {
                LevelUp(_exp / (10 * _lv));
                _exp = _exp % (10 * _lv);
                return true;
            }
            return false;
        }

        private void LevelUp(int val)
        {
            _lv += val;
            _attackPower += 5;
            _def += 2;
            _hp.AddMaxValue(10);
        }

        public void Damaged(int amount)
        {
            int totalDmg = amount - _def;
            _hp.SubValue((totalDmg > 0) ? totalDmg : 1);
        }

        public void Recover(int amount)
        {
            _hp.AddValue(amount);
        }

        public void SetHP(int curValue, int maxValue, int minValue)
        {
            _hp = new GaugeValue<int>(curValue, maxValue, minValue);
        }

        public int GetAttackValue()
        {
            return _attackPower;
        }

        public void Revive()
        {
            Recover(1000);
        }
    }
}
