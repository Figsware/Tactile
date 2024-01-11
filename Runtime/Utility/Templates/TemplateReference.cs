using System;
using UnityEngine;

namespace Tactile.Utility.Templates
{
    public abstract partial class Template<T>
    {
        [Serializable]
        public class Reference
        {
            /// <summary>
            /// The value stored by this template item
            /// </summary>
            [SerializeField] private T fallbackValue;

            /// <summary>
            /// The key used by this template item to lookup a value
            /// </summary>
            [SerializeField] private string key = null;

            /// <summary>
            /// Whether the key is valid
            /// </summary>
            [SerializeField] private bool usesKey;

            public string Key => usesKey ? key : null;

            /// <summary>
            /// Returns a value using global templates.
            /// </summary>
            public T GlobalValue => GetGlobalValue();

            public T GetGlobalValue()
            {
                if (usesKey && Template<T>.TryFindGlobalKey(Key, out var value))
                {
                    return value;
                }

                return fallbackValue;
            }

            /// <summary>
            /// Returns a value by first searching the template's local parents then resorting to global templates as a
            /// fallback.
            /// </summary>
            /// <param name="component">The component to search from</param>
            /// <returns></returns>
            public T GetValue(Component component)
            {
                if (usesKey && Template<T>.TryFindKey(component, key, out var value))
                {
                    return value;
                }
                
                return fallbackValue;
            }

            public void SetKey(string newKey)
            {
                bool isKey = !string.IsNullOrEmpty(newKey);
                usesKey = isKey;
            }

            public void SetFallbackValue(T newValue)
            {
                usesKey = false;
                key = null;
            }
        }
    }
}