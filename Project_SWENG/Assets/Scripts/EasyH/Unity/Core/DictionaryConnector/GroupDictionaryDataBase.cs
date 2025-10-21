using System;
using System.Collections.Generic;
using UnityEngine;

namespace EasyH {

    public abstract class GroupDictionaryDataBase<K, V> : ScriptableObject
    {

        [Serializable]
        public class DictionaryDataUnitGroup
        {
            public string GroupName;
            public SerializableDictionary<K, V> Data;
        }

        public List<DictionaryDataUnitGroup> Data;

        public IDictionary<K, V> GetDictionary()
        {
            IDictionary<K, V> dic = new Dictionary<K, V>();

            foreach (var g in Data)
            {
                foreach (var v in g.Data)
                {
                    dic.Add(v.Key, v.Value);
                }
            }

            return dic;

        }
    }
}