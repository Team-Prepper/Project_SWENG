using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character {
    [System.Serializable]
    public class Stat {

        public int Lv;
        public int Exp;
        public GaugeValue<int> HP = new GaugeValue<int>(10, 10, 0);
        public int attackPower;

    }
}
