using UnityEngine;

namespace Tactile.Utility.Templates
{
    public static class TactileTemplateUtility
    {
        public static T? GetNullableTemplateItemValue<T>(this UnityNullable<Template<T>.Reference> unityNullable, Component component)
            where T : struct
        {
            T? nullableValue = null;
            
            if (unityNullable.HasValue)
                nullableValue = unityNullable.Value.GetValue(component);
            
            return nullableValue;
        }

        public static void SetNullableTemplateItemValue<T>(this UnityNullable<Template<T>.Reference> unityNullable,
            T? nullableValue)
            where T : struct
        {
            if (nullableValue is { } value)
            {
                unityNullable.Value.SetFallbackValue(value);
            }
            else
            {
                unityNullable.Value = null;
            }
        }
    }
}