using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.InputSystem;

public class JsonDataConnector<K, V> {

    string _path;
    IDictionary<K, V> _dict;

    public  void Connect(string name)
    {
#if UNITY_EDITOR
        _path = string.Format("{0}/{1}/{2}.json", Application.dataPath, "/Resources", name);
#else
        _path = string.Format("{0}/{1}.json", Application.persistentDataPath, name);
#endif

    }

    public IDictionary<K, V> GetAllData() {

        if (_dict == null)
        {
            string json;
            if (File.Exists(_path))
                json = File.ReadAllText(_path);
            else
            {
                json = "{\"value\":[]}";
            }
            _dict = JsonConvert.DeserializeObject<Dictionary<K, V>>(json);
        }

        return _dict;
    }

    public V Get(K key)
    {
        return GetAllData()[key];
    }

    public void Set(K key, V value)
    {
        if (_dict.ContainsKey(key))
        {
            _dict[key] = value;
        }
        else {
            _dict.Add(key, value);
        }

        string json = JsonConvert.SerializeObject(_dict, Formatting.Indented);

        File.WriteAllText(_path, json);

    }

    public void Set(IDictionary<K, V> dic)
    {
        _dict = dic;

        string json = JsonConvert.SerializeObject(_dict, Formatting.Indented);

        File.WriteAllText(_path, json);
    }
}
