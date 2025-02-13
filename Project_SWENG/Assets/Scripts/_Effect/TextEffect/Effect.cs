using System.Collections;
using System.Collections.Generic;
using TMPro.Examples;
using UnityEngine;
using UnityEngine.Events;

public abstract class Effect : MonoBehaviour
{
    [SerializeField] protected float _effectTime;
    [SerializeField] UnityEvent _eventWhenEffectEnd;

    public abstract void On(Vector3 pos);

    protected void EndEffect() { 
        _eventWhenEffectEnd.Invoke();
    }

    /*
    public static void PlayEffect(string key, Vector3 position, Transform parent)
    {
        Effect effect = ObjectPool.Instance.GetGameObject(key).GetComponent<Effect>();
        effect.On(position);
        effect.transform.SetParent(parent);

    }

    public static void PlayEffect(string key, Transform targetTr)
    {
        PlayEffect(key, targetTr.position, targetTr.parent);

    }
    */
}
