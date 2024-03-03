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

        private void Start()
        {
            if (layout)
            {
                BuildLayout(layout);
            }
        }

        public void BuildLayout(MenuLayout newLayout)
        {
            var items = CreateItems(newLayout.items);
            foreach (var menuBuilder in builders)
            {
                menuBuilder.SetMenuItems(items);
            }
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