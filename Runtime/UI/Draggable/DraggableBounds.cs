using UnityEngine;

namespace Tactile.UI.Menu
{
    public abstract class DraggableBounds : MonoBehaviour
    {
        public abstract Vector2 GetInitialPosition(RectTransform draggableRt);
        
        public abstract Vector2 ConstrainPosition(Vector2 position, RectTransform draggableRt);
    }
}