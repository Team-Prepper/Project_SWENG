using System.Collections.Generic;
using EasyH.Unity;

namespace EasyH {

    public class ScriptableObjectDictionaryConnector<K, V> : IDictionaryConnector<K, V>
    {
        public string GetDefaultPath() => "Data";
        public string GetExtensionName() => "";

        public IDictionary<K, V> ReadData(string path)
        {
            DictionaryDataBase<K, V> data
                = ResourceManager.Instance.ResourceConnector.
                    Import<DictionaryDataBase<K, V>>(path);

            return data.GetDictionary();
        }

        public void Save(IDictionary<K, V> data, string path)
        {
            
        }
    }
}