using EHTool.UtilKit;
using UnityEngine;

public abstract class IHealthUI : MonoBehaviour
{
    public abstract void UpdateGUI(GaugeValue<int> value);
}
