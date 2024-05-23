using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace Tactile.UI.Menu
{
    public class MenuStateManager : MonoBehaviour
    {
        [Tooltip("Called when any state gets invoked.")]
        public UnityEvent<string> OnStateKeyInvoke;

        public List<MenuState> states;

        public MenuState CreateOrRetrieveState(string key)
        {
            var existingState = states.FirstOrDefault(s => s.key == key);
            if (existingState != null)
                return existingState;

            var newState = new MenuState()
            {
                key = key
            };
            
            // Add a listener that will forward the invoke event.
            newState.onStateInvoke.AddListener(() => OnStateKeyInvoke.Invoke(key));
            
            states.Add(newState);

            return newState;
        }
    }
}