using System.ComponentModel;
using UnityEngine;
using UnityEngine.Events;

namespace Tactile.UI
{
    public class UIActionConfigurator : MonoBehaviour
    {
        public UnityEvent<string> OnNewName;
        public UnityEvent<string> OnNewDescription;
        public UnityEvent<Texture> OnNewIcon;
        public UnityEvent<bool> OnDisabledChanged;
        public UnityEvent<Color> OnNewColor;

        private UIAction _uiAction;
        
        public void SetUIAction(UIAction uiAction)
        {
            if (_uiAction != null)
            {
                _uiAction.PropertyChanged -= OnUIActionPropertyChange;
            }
            
            _uiAction = uiAction;
            _uiAction.PropertyChanged += OnUIActionPropertyChange;
            
            OnNewName.Invoke(uiAction.Name);
            OnNewDescription.Invoke(uiAction.Description);
            OnNewIcon.Invoke(uiAction.Icon);
            OnDisabledChanged.Invoke(uiAction.Disabled);
        }

        public void Select()
        {
            if (_uiAction == null)
                return;
            
            _uiAction.onSelect.Invoke();
        }

        private void OnUIActionPropertyChange(object sender, PropertyChangedEventArgs args)
        {
            
        }
    }
}