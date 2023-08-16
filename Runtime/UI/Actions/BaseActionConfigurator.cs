using System;
using System.ComponentModel;
using UnityEngine;

namespace Tactile.UI
{
    public abstract class BaseActionConfigurator : MonoBehaviour
    {
        [SerializeField] private UIAction action;

        protected UIAction Action => action;

        private void Start()
        {
            SetUIAction(action);
        }

        public virtual void SetUIAction(UIAction newAction)
        {
            if (action != null)
            {
                action.PropertyChanged -= OnUIActionPropertyChange;
            }

            action = newAction;
            action.PropertyChanged += OnUIActionPropertyChange;
        }

        private void OnValidate()
        {
            OnUIActionPropertyChange(action, default);
        }

        protected abstract void OnUIActionPropertyChange(object sender, PropertyChangedEventArgs args);
    }
}