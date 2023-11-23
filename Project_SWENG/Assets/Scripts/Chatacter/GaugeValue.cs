using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class GaugeValue<T> where T: IComparable, IConvertible {

    [SerializeField] private T _curValue;
    [SerializeField] private T _maxValue;
    [SerializeField] private T _minValue;

    public T Value {
        get { 
            return _curValue; 
        }
    }

    public T MaxValue {
        get {
            return _maxValue;
        }
    }

    public GaugeValue() {
        _curValue = default;
        _maxValue = default;
        _minValue = default;
    }

    public GaugeValue(T curValue)
    {
        _curValue = curValue;
        _maxValue = curValue;
        _minValue = default;
    }

    public GaugeValue(T curValue, T maxValue) : this(curValue)
    {
        _maxValue = maxValue;
    }

    public GaugeValue(T curValue, T maxValue, T minValue) : this(curValue, maxValue)
    {
        _minValue = minValue;
    }

    public void AddMaxValue(T addAmount) {
        _maxValue = (_maxValue.ConvertTo<double>()
                     + addAmount.ConvertTo<double>()).ConvertTo<T>();
        
        _curValue = (_curValue.ConvertTo<double>()
                     + addAmount.ConvertTo<double>()).ConvertTo<T>();

        if (_curValue.CompareTo(_maxValue) > 0)
            _curValue = _maxValue;
    }

    public void SubMaxValue(T addAmount)
    {
        _maxValue = (_maxValue.ConvertTo<double>()
                     - addAmount.ConvertTo<double>()).ConvertTo<T>();
    }

    public void AddValue(T addAmount) {
        _curValue = (_curValue.ConvertTo<double>()
            + addAmount.ConvertTo<double>()).ConvertTo<T>();

        if (_curValue.CompareTo(_maxValue) > 0)
            _curValue = _maxValue;
    }

    public void SubValue(T subAmount)
    {
        _curValue = (_curValue.ConvertTo<double>()
            - subAmount.ConvertTo<double>()).ConvertTo<T>();

        if (_curValue.CompareTo(_minValue) < 0)
            _curValue = _minValue;

    }

    public void FillMax() {
        _curValue = _maxValue;
    }

    public void SetValueMin() {
        _curValue = _minValue;
    }

    public float ConvertToRate()
    {
        return (_curValue.ConvertTo<float>() - _minValue.ConvertTo<float>())
            / (_maxValue.ConvertTo<float>() - _minValue.ConvertTo<float>());
    }

    public override string ToString()
    {
        return _curValue.ToString() + "/" + _maxValue.ToString();
    }

}
