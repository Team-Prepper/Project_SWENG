using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHealthUI
{
    public void UpdateGUI(GaugeValue<int> value);
}
