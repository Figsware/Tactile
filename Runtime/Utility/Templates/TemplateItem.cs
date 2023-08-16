using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Tactile.Utility.Templates
{
    [Serializable]
    public class TemplateItem<T>: INotifyPropertyChanged
    {
        [SerializeField] private T value;
        [SerializeField] private string key = null;
        [SerializeField] private bool usesKey;
        

        public event PropertyChangedEventHandler PropertyChanged;

        public void SetKey(string newKey)
        {
            bool isKey = !string.IsNullOrEmpty(newKey);
            usesKey = isKey;
            SetField(ref key, isKey ? newKey : null);
        }

        public void SetValue(T newValue)
        {
            usesKey = false;
            key = null;
            SetField(ref value, newValue);
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        public static implicit operator T(TemplateItem<T> item) => item.value;
    }
}