using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace Tactile.Utility
{
    /// <summary>
    /// A unity dictionary can be configured within the editor and referred to as a readonly
    /// dictionary.
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    [Serializable]
    public class UnityDictionary<TKey, TValue> : IDictionary<TKey, TValue>, ISerializationCallbackReceiver, IUnityDictionary
    {
        [SerializeField] private DictionaryItem[] items;

        private Dictionary<TKey, TValue> _dictionary;

        public UnityDictionary()
        {
            _dictionary = new Dictionary<TKey, TValue>();
        }

        public IDictionary<TKey, TValue> GetDictionary()
        {
            return _dictionary;
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() => GetDictionary().GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetDictionary().GetEnumerator();
        public void Add(KeyValuePair<TKey, TValue> item) => GetDictionary().Add(item);
        public void Clear() => GetDictionary().Clear();
        public bool Contains(KeyValuePair<TKey, TValue> item) => GetDictionary().Contains(item);
        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex) => GetDictionary().CopyTo(array, arrayIndex);
        public bool Remove(KeyValuePair<TKey, TValue> item) => GetDictionary().Remove(item);
        public int Count => GetDictionary().Count;
        public bool IsReadOnly => GetDictionary().IsReadOnly;
        public void Add(TKey key, TValue value) => GetDictionary().Add(key, value);
        public bool ContainsKey(TKey key) => GetDictionary().ContainsKey(key);
        public bool Remove(TKey key) => GetDictionary().Remove(key);
        public bool TryGetValue(TKey key, out TValue value) => GetDictionary().TryGetValue(key, out value);
        public TValue this[TKey key]
        {
            get => GetDictionary()[key];
            set => GetDictionary()[key] = value;
        }
        public ICollection<TKey> Keys => GetDictionary().Keys;
        public ICollection<TValue> Values => GetDictionary().Values;

        [Serializable]
        public class DictionaryItem
        {
            public TKey key;
            public TValue value;

            public DictionaryItem(TKey key, TValue value)
            {
                this.key = key;
                this.value = value;
            }
        }

        public void OnBeforeSerialize()
        {
            if (items == null)
            {
                items = new DictionaryItem[_dictionary.Count];
                int i = 0;
                foreach (var (key, value) in _dictionary)
                {
                    items[i++] = new DictionaryItem(key, value);
                }
            }
        }
        
        public void OnAfterDeserialize()
        {
            if (items != null)
            {
                _dictionary = new Dictionary<TKey, TValue>();

                bool isValid = true;
                foreach (var item in items)
                {
                    isValid = isValid && _dictionary.TryAdd(item.key, item.value);
                }
            
                // Get rid of the items so that it doesn't hog memory.
                if (isValid)
                {
                    items = null;
                }
            }
        }

        public bool IsValidConfiguration()
        {
            return items == null;
        }
    }

    public interface IUnityDictionary
    {
        bool IsValidConfiguration();
    }
}