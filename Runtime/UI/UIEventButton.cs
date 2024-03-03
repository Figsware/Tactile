using UnityEngine.EventSystems;

namespace Tactile.UI
{
    public class UIEventButton: ButtonBase, IPointerDownHandler, IPointerUpHandler
    {
        public override event IButton.ButtonPressHandler OnButtonStateChanged;
        public override bool IsPressed => _isPressed;
        private bool _isPressed;
        
        public void OnPointerDown(PointerEventData eventData)
        {
            _isPressed = true;
            OnButtonStateChanged?.Invoke(true);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            _isPressed = false;
            OnButtonStateChanged?.Invoke(false);
        }
    }
}