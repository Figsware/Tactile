namespace Tactile.UI
{
    public interface IButton
    {
        public delegate void ButtonPressHandler(bool isPressed);
        public event ButtonPressHandler OnButtonStateChanged;
        public bool IsPressed { get; }
    }
}