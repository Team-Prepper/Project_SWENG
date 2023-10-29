using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character {
    [System.Serializable]
    public class Stat {

        private int _lv;
        private int _exp;
        private GaugeValue<int> _hp = new GaugeValue<int>(100, 100, 0);
        private int _attackPower;

        public GaugeValue<int> HP {
            get {
                return _hp;
            }
        }

        public bool IsAlive()
        {
            return _hp.Value != 0;
        }

        public void Damaged(int amount)
        {
            _hp.SubValue(amount);
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
    }
}
