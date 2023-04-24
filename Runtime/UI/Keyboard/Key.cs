using UnityEngine;
using UnityEngine.Events;

namespace Tactile.UI.Keyboard
{
    public class Key : MonoBehaviour
    {
        public UnityEvent onKeyDown;
        public UnityEvent onKeyUp;
        
        [SerializeField] private UnityEvent<string> onKeyTextSet;
        [SerializeField] private UnityEvent onKeyActivate;
        [SerializeField] private UnityEvent onKeyDeactivate;

        public void SetKeyText(string key)
        {
            onKeyTextSet.Invoke(key);
        }

        public void EmitKeyDown()
        {
            onKeyDown.Invoke();
        }

        public void EmitKeyUp()
        {
            onKeyUp.Invoke();
        }

        public void SetKeyActivated(bool activated)
        {
            var ue = activated ? onKeyActivate : onKeyDeactivate;
            ue.Invoke();
        }

        public void SetColor(Color color)
        {
            
        }
    }
}