using UnityEngine;

namespace Tactile.Utility.Templates
{
    public static class TactileTemplateUtility
    {
        public static T? GetNullableTemplateItemValue<T>(this UnityNullable<KeyReference<T>> unityNullable, Component component)
            where T : struct
        {
            T? nullableValue = null;
            
            if (unityNullable.HasValue)
                nullableValue = unityNullable.Value.GetTemplateValue(component);
            
            return nullableValue;
        }

        public static void SetNullableTemplateItemValue<T>(this UnityNullable<KeyReference<T>> unityNullable,
            T? nullableValue)
            where T : struct
        {
            if (nullableValue is { } value)
            {
                unityNullable.Value.SetInlineValue(value);
            }
            else
            {
                unityNullable.Value = null;
            }
        }
        

        public static T GetGlobalTemplateValue<T>(this KeyReference<T> keyReference)
        {
            if (keyReference.UsingKey && Template<T>.TryFindGlobalKey(keyReference.Key, out var value))
            {
                return value;
            }

            return keyReference.InlineValue;
        }

        /// <summary>
        /// Returns a value by first searching the template's local parents then resorting to global templates as a
        /// fallback.
        /// </summary>
        /// <param name="keyReference"></param>
        /// <param name="component">The component to search from</param>
        /// <returns></returns>
        public static T GetTemplateValue<T>(this KeyReference<T> keyReference, Component component)
        {
            if (keyReference.UsingKey && Template<T>.TryFindKey(component, keyReference.Key, out var value))
            {
                return value;
            }
                
            return keyReference.InlineValue;
        }
    }
}