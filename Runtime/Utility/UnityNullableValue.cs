#nullable enable
using System;
using UnityEngine;

namespace Tactile.Utility
{
    [Serializable]
    public class UnityNullableValue<T> : UnityNullable<T> where T : struct
    {
        public UnityNullableValue()
        {
            hasValue = false;
            value = default;
        }

        public UnityNullableValue(T? value)
        {
            SetNullableValue(value);
        }

        public T? NullableValue
        {
            get => HasValue ? Value : null;
            set
            {
                if (value.HasValue)
                {
                    SetValue(value.Value);
                }
                else
                {
                    hasValue = false;
                }
            }
        }

        public void SetNullableValue(T? nullableValue)
        {
            if (nullableValue.HasValue)
            {
                hasValue = true;
                value = nullableValue.Value;
            }
            else
            {
                hasValue = false;
                value = default;
            }
        }

        public static implicit operator UnityNullableValue<T>(T? nullableValue)
        {
            var unv = new UnityNullableValue<T>();
            unv.SetNullableValue(nullableValue);

            return unv;
        }

        public static implicit operator T?(UnityNullableValue<T> unityNullableValue) =>
            unityNullableValue.NullableValue;
    }
}