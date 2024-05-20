using System;
using UnityEngine;

namespace Tactile.Utility
{
    [Serializable]
    public class KeyReference<T>
    {
        /// <summary>
        /// The value stored by this reference.
        /// </summary>
        [SerializeField] private T inlineValue;

        /// <summary>
        /// The key used by this template item to lookup a value
        /// </summary>
        [SerializeField] private string key = null;

        /// <summary>
        /// Whether the key is being used.
        /// </summary>
        [SerializeField] private bool usesKey;

        public string Key => usesKey ? key : null;
        public bool UsingKey => usesKey;
        public T InlineValue => inlineValue;

        public KeyReference()
        {
            SetKey(null);
        }

        public KeyReference(T inlineValue)
        {
            SetInlineValue(inlineValue);
        }

        public KeyReference(string key)
        {
            SetKey(key);
        }

        public void SetKey(string newKey)
        {
            bool isKey = !string.IsNullOrEmpty(newKey);
            usesKey = isKey;
            key = newKey;

            if (!isKey)
                inlineValue = default;
        }

        public void SetInlineValue(T newValue)
        {
            usesKey = false;
            key = null;
            inlineValue = newValue;
        }
    }
}