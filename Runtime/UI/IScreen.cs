using UnityEngine;

namespace Tactile.UI.Menu
{
    public interface IScreen
    {
        public string Title { get; }
        public string Key { get; }
        public Texture Icon { get; }
    }
}