using System;
using Tactile.Utility.Settings;
using UnityEngine;
using UnityEngine.Events;

namespace Tactile.UI.Menu
{
    /// <summary>
    /// Represents the state of some item within a menu. Every menu state can be disabled, which should indicate to
    /// the user that it is unavailable to be modified.
    /// </summary>
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

    /// <summary>
    /// Actions are state that can be invoked to trigger some other action. This can be doing so via the OnActionInvoke
    /// event.
    /// </summary>
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

    /// <summary>
    /// A setting is menu state that holds some value that can be modified. You should prefer types that are non-mutable
    /// so that updates to the state can be readily made available.
    /// </summary>
    /// <typeparam name="T"></typeparam>
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