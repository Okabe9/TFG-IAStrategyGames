using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SerializableKeyValuePair<TKey, TValue>
{
  public TKey Key;
  public TValue Value;
}

[Serializable]
public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
{
  [SerializeField] private List<SerializableKeyValuePair<TKey, TValue>> keyValuePairs = new List<SerializableKeyValuePair<TKey, TValue>>();

  public SerializableDictionary(IEqualityComparer<TKey> comparer) : base(comparer)
  {
  }
  public void OnBeforeSerialize()
  {
    keyValuePairs.Clear();

    foreach (KeyValuePair<TKey, TValue> pair in this)
    {
      keyValuePairs.Add(new SerializableKeyValuePair<TKey, TValue>() { Key = pair.Key, Value = pair.Value });
    }
  }

  public void OnAfterDeserialize()
  {
    this.Clear();

    foreach (SerializableKeyValuePair<TKey, TValue> pair in keyValuePairs)
    {
      this.Add(pair.Key, pair.Value);
    }
  }

}