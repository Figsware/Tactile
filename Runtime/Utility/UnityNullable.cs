#nullable enable
using System;
using UnityEngine;

namespace Tactile.Utility
{
    [Serializable]
    public class UnityNullable<T>
    {
        [SerializeField] protected bool hasValue;
        [SerializeField] protected T value;
        
        public UnityNullable()
        {
            hasValue = false;
            value = default;
        }

        public UnityNullable(T value)
        {
            SetValue(value);
        }

        public bool HasValue
        {
            get => hasValue;
        }

        public T Value
        {
            get => value;
            set => SetValue(value);
        }

        public T? GetValueOrDefault() => hasValue ? value : default;
        
        public void SetValue(T newValue)
        {
            if (newValue == null)
            {
                hasValue = false;
                value = default;
            }
            else
            {
                hasValue = true;
                value = newValue;
            }
        }

        public void SetReferenceIfHasValue(ref T valueReference)
        {
            if (HasValue)
            {
                valueReference = value;
            }
        }

        public static implicit operator T(UnityNullable<T> unityNullable) => unityNullable.Value;

        public static implicit operator UnityNullable<T>(T value) => new(value);
    }
}