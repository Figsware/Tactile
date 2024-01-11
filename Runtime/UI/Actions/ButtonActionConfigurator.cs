using System;
using System.ComponentModel;
using UnityEngine.UI;
using UnityEngine;

namespace Tactile.UI
{
    public class ButtonActionConfigurator : BaseActionConfigurator
    {
        [SerializeField] private Button button;

        private void Awake()
        {
            SetButton(button);
        }

        protected override void OnUIActionPropertyChange(object sender, PropertyChangedEventArgs args)
        {
            // TODO:  change parameters on an individual basis rather than all of them all at once?
            ConfigureButton();
        }

        private void ConfigureButton()
        {
            button.interactable = !Action.Disabled;
            button.colors = Action.SetColorBlock(button.colors, this);
        }

        private void OnButtonSelect()
        {
            Action.onSelect.Invoke();
        }

        private void SetButton(Button newButton)
        {
            if (button)
            {
                button.onClick.RemoveListener(OnButtonSelect);
            }

            button = newButton;
            if (newButton)
            {
                newButton.onClick.AddListener(OnButtonSelect);
            }
        }
    }
}