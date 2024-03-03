using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Tactile.UI.Menu.Navigation
{
    [AddComponentMenu("Tactile/UI/Screen Manager")]
    public class ScreenManager : MonoBehaviour
    {
        [SerializeField] private GameObject screenParent;
        [SerializeField] private bool stopCoroutinesOnHide = true;
        [SerializeField] private int currentScreenIndex;

        public UnityEvent<int> onNewScreenIndex;

        public int CurrentScreenIndex => currentScreenIndex;
        
        public IScreen[] Screens => GetScreens();
        private Screen _currentScreen = null;

        private void Awake()
        {
            if (screenParent == null)
            {
                screenParent = gameObject;
            }
        
            SetScreenFromCurrentIndex();
        }

        private void OnValidate()
        {
            SetScreenFromCurrentIndex();
        }

        public void ShowScreen(string key)
        {
            var screens = GetScreens();
            for (int i = 0; i < screens.Length; i++)
            {
                var screen = screens[i];
                if (screen.Key == key)
                {
                    ShowScreen(i);
                    return;
                }
            }
            
            Debug.LogError($"Tried to show a screen with the key \"{key}\" that doesn't exist!", this);
        }

        public void ShowScreen(int newScreenIndex)
        {
            var screens = GetScreens();
            if (0 <= newScreenIndex && newScreenIndex < screens.Length)
            {
                foreach (var screen in screens)
                {
                    screen.gameObject.SetActive(false);
                    
                    if (screen == _currentScreen)
                    {
                        if (stopCoroutinesOnHide)
                            _currentScreen.StopAllCoroutines();
                        
                        _currentScreen.onDisappear.Invoke();
                    }
                }

                currentScreenIndex = newScreenIndex;
                _currentScreen = screens[currentScreenIndex];
                _currentScreen.gameObject.SetActive(true);
                _currentScreen.onAppear.Invoke();
                onNewScreenIndex.Invoke(currentScreenIndex);
            }
            else
            {
                Debug.LogError($"Tried to show a screen with index {newScreenIndex} that doesn't exist!", this);
            }
        }

        private void SetScreenFromCurrentIndex()
        {
            ShowScreen(currentScreenIndex);
        }

        private Screen[] GetScreens()
        {
            var parent = screenParent ? screenParent : gameObject;
            List<Screen> screens = new List<Screen>();
            for (int i = 0; i < parent.transform.childCount; i++)
            {
                var child = parent.transform.GetChild(i);
                if (child.GetComponent<Screen>() is { } screen)
                {
                    screens.Add(screen);
                }
            }

            return screens.ToArray();
        }
    }
}