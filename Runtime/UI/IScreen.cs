using UnityEngine;

namespace Tactile.UI
{
    public interface IScreen
    {
        public string Title { get; }
        public string Key { get; }
        public Texture2D Icon { get; }
    }
}