using System;
using UnityEngine;
using UnityEngine.Events;

namespace Tactile.UI.Menu.Navigation
{
    public class TabButton : MonoBehaviour
    {
        public delegate void TabBarSelectedHandler(TabButton button);
        public event TabBarSelectedHandler OnSelect;

        [SerializeField] private UnityEvent onTabBarSelect;
        [SerializeField] private UnityEvent onTabBarDeselect;
        [SerializeField] private UnityEvent<Texture> onSetIcon;
        [SerializeField] private UnityEvent<string> onSetText;

        public void SetScreen(IScreen screen)
        {
            onSetText.Invoke(screen.Title);
            onSetIcon.Invoke(screen.Icon);
        }
        
        public void Select()
        {
            OnSelect?.Invoke(this);
        }

        public void SetSelected(bool selected)
        {
            (selected ? onTabBarSelect : onTabBarDeselect).Invoke();
        }
    }
}