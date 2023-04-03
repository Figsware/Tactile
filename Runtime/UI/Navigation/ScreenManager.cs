using UnityEngine;
using UnityEngine.Events;

namespace Tactile.UI.Navigation
{
    [AddComponentMenu("Tactile/UI/Screen Manager")]
    public class ScreenManager : MonoBehaviour
    {
        [SerializeField] private GameObject screenParent;
        [SerializeField] private bool stopCoroutinesOnHide = true;
        [SerializeField] private int currentScreenIndex;

        public UnityEvent<IScreen> onHideScreen;
        public UnityEvent<IScreen> onNewScreen;
        public IScreen[] Screens => ScreenObjects;

        private Screen[] ScreenObjects => (screenParent ? screenParent : gameObject).GetComponentsInChildren<Screen>(true);
        private Screen currentScreen = null;

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
            for (int i = 0; i < ScreenObjects.Length; i++)
            {
                var screen = ScreenObjects[i];
                if (screen.key == key)
                {
                    ShowScreen(i);
                    break;
                }
            }
        }

        public void ShowScreen(int newScreenIndex)
        {
            if (0 <= newScreenIndex && newScreenIndex < ScreenObjects.Length)
            {
                foreach (var screen in ScreenObjects)
                {
                    screen.gameObject.SetActive(false);
                    
                    if (screen == currentScreen)
                    {
                        if (stopCoroutinesOnHide)
                            currentScreen.StopAllCoroutines();
                    
                        onHideScreen.Invoke(screen);
                        currentScreen.onDisappear.Invoke();
                    }
                }

                currentScreenIndex = newScreenIndex;
                currentScreen = ScreenObjects[currentScreenIndex];
                currentScreen.gameObject.SetActive(true);
                currentScreen.onAppear.Invoke();
                onNewScreen.Invoke(currentScreen);
            }
        }

        private void SetScreenFromCurrentIndex()
        {
            ShowScreen(currentScreenIndex);
        }
    }
}