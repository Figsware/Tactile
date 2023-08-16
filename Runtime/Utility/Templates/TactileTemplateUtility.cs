namespace Tactile.Utility.Templates
{
    public static class TactileTemplateUtility
    {
        public static T? GetNullableTemplateItemValue<T>(this UnityNullable<TemplateItem<T>> unityNullable)
            where T : struct
        {
            T? nullableValue = null;
            
            if (unityNullable.HasValue)
                nullableValue = unityNullable.Value;
            
            return nullableValue;
        }

        public static void SetNullableTemplateItemValue<T>(this UnityNullable<TemplateItem<T>> unityNullable,
            T? nullableValue)
            where T : struct
        {
            if (nullableValue is { } value)
            {
                unityNullable.Value.SetValue(value);
            }
            else
            {
                unityNullable.Value = null;
            }
        }
    }
}