using System;
using System.Collections.Generic;
using UnityEngine;

namespace EasyH {
    
    public abstract class DictionaryDataBase<K, V> : ScriptableObject
    {
        public SerializableDictionary<K, V> Data;
        
        public IDictionary<K, V> GetDictionary()
        {
            return Data;
        }
    }
}