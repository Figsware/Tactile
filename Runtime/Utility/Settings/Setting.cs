using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;

namespace Tactile.Utility.Settings
{
    [Serializable]
    public class Setting<T> : INotifyPropertyChanged, ISettingChangedNotifier
    {
        [SerializeField] private T value;
        [SerializeField] private UnityEvent<T> onValueChange;

        public delegate void SettingChangedHandler(T newValue);
        
        public event PropertyChangedEventHandler PropertyChanged;

        public T Value
        {
            get => value;
            set => SetValue(value);
        }

        public void SetValue(T newValue)
        {
            if (EqualityComparer<T>.Default.Equals(newValue, value)) return;
            
            value = newValue;
            onValueChange?.Invoke(value);
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(value)));
        }

        public void AddChangeListener(UnityAction<T> action)
        {
            onValueChange.AddListener(action);
        }

        public void RemoveChangeListener(UnityAction<T> action)
        {
            onValueChange.RemoveListener(action);
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            if (propertyName == nameof(value))
                onValueChange?.Invoke(value);
        }

        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        public static implicit operator T(Setting<T> setting) => setting.value;

        void ISettingChangedNotifier.SendChangeEvent() => onValueChange.Invoke(value);
    }

    public interface ISettingChangedNotifier
    {
        void SendChangeEvent();
    }
}