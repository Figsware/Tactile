using UnityEngine;

namespace Tactile.UI
{
    public abstract class ButtonBase: MonoBehaviour, IButton
    {
        public abstract event IButton.ButtonPressHandler OnButtonStateChanged;
        public abstract bool IsPressed { get; }
    }
}