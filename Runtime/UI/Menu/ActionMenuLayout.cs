using System;
using UnityEngine;

namespace Tactile.UI.Menu
{
    [CreateAssetMenu(fileName = "Menu", menuName = "Tactile/Action Menu")]
    public class ActionMenuLayout : MenuLayout
    {
        [SerializeField] private ItemConfig[] items;
        
        [Serializable]
        public class ItemConfig
        {
            [SerializeField] public MenuStyle style;
            [SerializeField] public ActionConfig action;
            [SerializeField] public ActionMenuLayout subLayout;
        }
        
        [Serializable]
        public class ActionConfig
        {
            public string key;
            public bool disabled;

            public MenuActionState CreateActionState()
            {
                var action = new MenuActionState(key);
                action.Disabled.SetValue(disabled);

                return action;
            }
        }
        
        public override MenuObject[] GetMenuObjects() => GetMenuObjects(items);

        private static MenuObject[] GetMenuObjects(ItemConfig[] configs)
        {
            var menuItems = new MenuObject[configs.Length];

            for (var i = 0; i < configs.Length; i++)
            {
                var item = configs[i];
                if (item.subLayout)
                {
                    var subItems = GetMenuObjects(item.subLayout.items);
                    var folder = new MenuFolder(item.style, subItems);
                    menuItems[i] = folder;
                }
                else if (!string.IsNullOrEmpty(item.action.key))
                {
                    var state = item.action.CreateActionState();
                    menuItems[i] = new MenuItem(item.style, state);
                }
                else
                {
                    menuItems[i] = new MenuObject(item.style);
                }
            }

            return menuItems;
        }
    }
}