using System;
using Codice.Client.BaseCommands.Annotate;
using UnityEngine;

namespace Tactile.UI.Menu
{
    public class MenuLayoutLoader : MonoBehaviour
    {
        [SerializeField] private MenuLayout layout;
        [SerializeField] private MenuStateManager stateManager;
        [SerializeField] private MenuBuilder[] builders;

        [SerializeField] [Tooltip("Whether to build the menu layout on Start()")]
        private bool buildOnStart = true;

        private void Start()
        {
            if (buildOnStart)
            {
                BuildLayout();
            }
        }

        private void BuildLayout()
        {
            if (!layout)
            {
                Debug.LogError("Cannot build layout because No menu layout is loaded.");
                return;
            }

            var items = CreateItems(layout.items);
            foreach (var menuBuilder in builders)
            {
                menuBuilder.SetMenuItems(items);
            }
        }

        /// <summary>
        /// Builds a new specified layout.
        /// </summary>
        /// <param name="newLayout">The new layout to build.</param>
        public void BuildLayout(MenuLayout newLayout)
        {
            if (layout == newLayout) return;
            layout = newLayout;
            BuildLayout();
        }

        private MenuItem[] CreateItems(MenuLayout.ItemConfig[] configItems)
        {
            var menuItems = new MenuItem[configItems.Length];

            for (var i = 0; i < configItems.Length; i++)
            {
                var item = configItems[i];
                if (item.subLayout)
                {
                    var subItems = CreateItems(item.subLayout.items);
                    var folder = new MenuFolder(item.style, subItems);
                    menuItems[i] = folder;
                }
                else
                {
                    var state = stateManager.CreateOrRetrieveState(item.stateKey);
                    menuItems[i] = new MenuAction(item.style, state);
                }
            }

            return menuItems;
        }
    }
}