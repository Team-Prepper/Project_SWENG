using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character {
    [System.Serializable]
    public class Stat {

        private int _lv;
        private int _exp;
        private GaugeValue<int> _hp = new GaugeValue<int>(10, 10, 0);
        private int _attackPower;

        public GaugeValue<int> GetHP() {
            return _hp;
        }

        public void SetHP(int curValue, int maxValue, int minValue) {
            _hp = new GaugeValue<int>(curValue, maxValue, minValue);
        }

        public int GetAttackValue() {
            return _attackPower;
        }
    }
}
