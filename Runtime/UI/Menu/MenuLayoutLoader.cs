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

            var menuObjects = layout.GetMenuObjects();
            AddMenuState(menuObjects);
            foreach (var menuBuilder in builders)
            {
                menuBuilder.SetMenuObjects(menuObjects);
            }
        }

        private void AddMenuState(MenuObject[] menuObjects)
        {
            foreach (var obj in menuObjects)
            {
                switch (obj)
                {
                    case MenuItem item:
                        stateManager.AddState(item.State);
                        break;
                    case MenuFolder folder:
                        AddMenuState(folder.Items);
                        break;
                }
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
    }
}