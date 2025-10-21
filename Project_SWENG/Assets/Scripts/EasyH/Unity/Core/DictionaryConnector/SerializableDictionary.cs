using System;
using System.Collections.Generic;
using UnityEngine;

namespace EasyH
{

    [Serializable]
    public class KeyValuePair<K, V>
    {
        public K Key;
        public V Value;
    }

    [Serializable]
    public class SerializableDictionary<K, V> :
        Dictionary<K, V>, ISerializationCallbackReceiver
    {
        [SerializeField] private List<KeyValuePair<K, V>> _elements;

        public void OnBeforeSerialize()
        {
            if (Count < _elements.Count)
            {
                return;
            }

            _elements.Clear();

            foreach (var kv in this)
            {
                _elements.Add(new KeyValuePair<K, V>()
                {
                    Key = kv.Key,
                    Value = kv.Value
                });
            }
        }

        public void OnAfterDeserialize()
        {
            Clear();

            foreach (var kv in _elements)
            {
                if (!TryAdd(kv.Key, kv.Value))
                {
                    Debug.LogWarning($"Duplicate Key '{kv.Key}' found during deserialization. Ignoring subsequent entry.");
                }
            }
        }
    }

}