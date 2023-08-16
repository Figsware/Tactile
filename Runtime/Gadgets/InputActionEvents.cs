using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace Tactile.Gadgets
{
    public class InputActionEvents : MonoBehaviour
    {
        [SerializeField] private ActionEvent[] actionEvents;

        private void Awake()
        {
            ConfigureActions();            
        }

        private void ConfigureActions()
        {
            foreach (var action in actionEvents)
            {
                action.ConfigureAction();
            }
        }

        [Serializable]
        public class ActionEvent
        {
            [SerializeField] private InputActionProperty actionProperty;
            [SerializeField] private UnityEvent<InputAction.CallbackContext> onStarted;
            [SerializeField] private UnityEvent<InputAction.CallbackContext> onPerformed;
            [SerializeField] private UnityEvent<InputAction.CallbackContext> onCanceled;

            public void ConfigureAction()
            {;
                actionProperty.action.started += onStarted.Invoke;
                actionProperty.action.performed += onPerformed.Invoke;
                actionProperty.action.canceled += onCanceled.Invoke;
            }
        }
    }
}