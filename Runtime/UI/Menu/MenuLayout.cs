using UnityEngine;

namespace Tactile.UI.Menu
{
    /// <summary>
    /// A Menu Layout is a Scriptable Object that facilitates the creation of menu objects. Note that, if you are trying
    /// to create menus from software, there is no need to programatically create a menu layout scriptable object.
    /// Instead, you should create the MenuObjects directly.
    /// </summary>
    public abstract class MenuLayout : ScriptableObject
    {
        public abstract MenuObject[] GetMenuObjects();
    }
}