using UnityEngine;

namespace Tactile.UI.Menu
{
    public abstract class MenuLayout : ScriptableObject
    {
        public abstract MenuObject[] GetMenuObjects();
    }
}