using Tactile.UI.Menu.Interfaces;
using UnityEngine;

namespace Tactile.UI.Menu
{
    public abstract class ButtonBase: MonoBehaviour, IButton
    {
        public abstract event IButton.ButtonPressHandler OnButtonStateChanged;
        public abstract bool IsPressed { get; }
    }
}