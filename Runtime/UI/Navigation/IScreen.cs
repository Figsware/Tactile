using UnityEngine;

namespace Tactile.UI.Navigation
{
    public interface IScreen
    {
        public string Title { get; }
        public string Key { get; }
        public Texture Icon { get; }
    }
}