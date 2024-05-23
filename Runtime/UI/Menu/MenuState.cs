using System;
using Tactile.Utility.Settings;
using UnityEngine;
using UnityEngine.Events;

namespace Tactile.UI.Menu
{
    public class MenuState
    {
        public readonly string Key;
        private Setting<bool> _disabled = new(false);
        public Setting<bool> Disabled => _disabled;

        public MenuState(string key)
        {
            Key = key;
        }
    }

    public class MenuActionState : MenuState
    {
        public delegate void ActionInvokedHandler();

        public event ActionInvokedHandler OnActionInvoke;

        public void Invoke()
        {
            OnActionInvoke?.Invoke();
        }

        public MenuActionState(string key) : base(key)
        {
        }
    }

    public class MenuSettingState<T> : MenuState
    {
        private readonly Setting<T> _value;

        public MenuSettingState(string key) : base(key)
        {
            _value = new Setting<T>();
        }

        public MenuSettingState(string key, T initialValue) : base(key)
        {
            _value = new Setting<T>(initialValue);
        }

        public T GetValue() => _value;
        public void SetValue(T newValue) => _value.SetValue(newValue);
        public void AddValueChangeListener(UnityAction<T> action) => _value.AddChangeListener(action);
        public void RemoveValueChangeListener(UnityAction<T> action) => _value.AddChangeListener(action);
    }
}