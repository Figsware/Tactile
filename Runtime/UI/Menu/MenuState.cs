using System;
using UnityEngine;
using UnityEngine.Events;

namespace Tactile.UI.Menu
{
    [Serializable]
    public class MenuState
    {
        public enum StateType
        {
            Button,
            Toggle,
            RadioGroup
        }

        public UnityEvent onStateInvoke = new();
        
        [SerializeField] public string key;
        [SerializeField] public StateType stateType;
        [SerializeField] public string radioGroupKey;
        [SerializeField] public bool disabled;
        [SerializeField] public bool state;

        public void Invoke()
        {
            if (stateType == StateType.Toggle)
                state = !state;
            
            onStateInvoke?.Invoke();
        }
    }
}