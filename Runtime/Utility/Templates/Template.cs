using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Tactile.Utility.Logging.Templates
{
    public abstract partial class Template<T> : MonoBehaviour, IReadOnlyDictionary<string, T>
    {
        public UnityDictionary<string, T> template;
        public bool IsGlobalTemplate = true;

        private static readonly List<Template<T>> CurrentTemplates = new List<Template<T>>();

        public IEnumerator<KeyValuePair<string, T>> GetEnumerator()
        {
            return template.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public int Count => template.Count;

        public bool ContainsKey(string key)
        {
            return template.ContainsKey(key);
        }

        public bool TryGetValue(string key, out T value)
        {
            return template.TryGetValue(key, out value);
        }

        public T this[string key] => template[key];

        public IEnumerable<string> Keys => template.Keys;
        public IEnumerable<T> Values => template.Values;

        private void Awake()
        {
            CurrentTemplates.Add(this);
        }

        private void OnDestroy()
        {
            CurrentTemplates.Remove(this);
        }

        public static bool TryFindGlobalKey(string key, out T value)
        {
            value = default;

            foreach (var template in CurrentTemplates.Where(t => t.IsGlobalTemplate))
            {
                if (template.TryGetValue(key, out value))
                    return true;
            }

            return false;
        }
        
        public static bool TryFindKey(Component component, string key, out T value)
        {
            var templates = component.GetComponentsInParent<Template<T>>();
            foreach (var template in templates)
            {
                if (template.TryGetValue(key, out value))
                {
                    return true;
                }
            }

            return TryFindGlobalKey(key, out value);
        }
    }
}