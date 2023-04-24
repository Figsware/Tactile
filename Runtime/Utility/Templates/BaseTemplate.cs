using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Tactile.Utility.Templates
{
    public abstract class BaseTemplate<T> : MonoBehaviour, IReadOnlyDictionary<string, T>
    {
        [SerializeField] private Asset asset;
        [SerializeField] private UnityDictionary<string, T> items;

        public abstract class Asset : ScriptableObject
        {
            [SerializeField] private UnityDictionary<string, T> items;
        }
        
        public abstract class Consumer: MonoBehaviour
        {
            [SerializeField] private BaseTemplate<T> template;
            protected BaseTemplate<T> Template => template;
        }

        public abstract class Applicator : Consumer
        {
            
        }

        public abstract class Picker : Consumer
        {
            [SerializeField] private string pickAtStart;
            [SerializeField] private UnityEvent<T> onItemPicked;

            private void Start()
            {
                if (!string.IsNullOrEmpty(pickAtStart))
                {
                    Pick(pickAtStart);
                }
            }

            public void Pick(string key)
            {
                if (Template.TryGetValue(key, out T value))
                {
                    onItemPicked.Invoke(value);
                }
                else
                {
                    Debug.LogError($"Tried to pick \"{key}\" but no such key exists!");
                }
            }
        }

        public IEnumerator<KeyValuePair<string, T>> GetEnumerator()
        {
            return items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public int Count => items.Count;
        public bool ContainsKey(string key)
        {
            return items.ContainsKey(key);
        }

        public bool TryGetValue(string key, out T value)
        {
            return items.TryGetValue(key, out value);
        }

        public T this[string key] => items[key];

        public IEnumerable<string> Keys => items.Keys;
        public IEnumerable<T> Values => items.Values;
    }
}