using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Tactile.UI.Menu
{
    public class MenuStateManager : MonoBehaviour
    {
        public UnityEvent<string> OnActionInvoke;
        private Dictionary<string, MenuState> _states = new();

        public void AddState(MenuState state)
        {
            if (state is MenuActionState actionState)
            {
                actionState.OnActionInvoke += () => OnActionInvoke.Invoke(state.Key);
            }
            
            _states[state.Key] = state;
        }
    }
}