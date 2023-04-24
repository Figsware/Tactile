using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Logger = Tactile.Utility.Logger;

namespace Tactile.UI.Navigation
{
    public class TabBar : MonoBehaviour
    {
        [SerializeField] private ScreenManager manager;
        [SerializeField] private HorizontalOrVerticalLayoutGroup buttonLayoutGroup;
        [SerializeField] private TabButton buttonPrefab;

        private TabButton[] _tabButtons;
        private Logger _logger;
        private int _prevScreenIndex;
        
        private void Awake()
        {
            _logger = new Logger(this);
            manager.onNewScreenIndex.AddListener(SelectTab);
            _prevScreenIndex = manager.CurrentScreenIndex;
        }

        private void OnValidate()
        {
            if (!Application.isPlaying)
                return;
            
            CreateTabs();
        }

        private void SelectTab(int index)
        {
            if (0 <= index && index < _tabButtons.Length)
            {
                // Deselect old tab
                _tabButtons[_prevScreenIndex].SetSelected(false);
                _prevScreenIndex = index;
                
                // Select new tab
                _tabButtons[index].SetSelected(true);
            }
            else
            {
                _logger.LogError($"Tried to select tab with index {index} that doesn't exist!");
            }
        }

        private void OnTabBarSelected(TabButton button)
        {
            int newIndex = 0;
            
            // Find the button that selected the tab.
            while (_tabButtons[newIndex] != button) newIndex++;
            
            manager.ShowScreen(newIndex);
        }
        
        private void CreateTabs()
        {
            buttonLayoutGroup.gameObject.DestroyAllChildren();
            _tabButtons = new TabButton[manager.Screens.Length];
            for (int i = 0; i < manager.Screens.Length; i++)
            {
                var screen = manager.Screens[i];
                var tabButton = Instantiate(buttonPrefab, buttonLayoutGroup.transform);
                tabButton.SetScreen(screen);
                tabButton.OnSelect += OnTabBarSelected;
                _tabButtons[i] = tabButton;
            }
        }
    }
}