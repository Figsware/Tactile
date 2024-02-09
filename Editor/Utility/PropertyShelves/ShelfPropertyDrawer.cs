using UnityEditor;
using UnityEngine;

namespace Tactile.Editor.Utility.PropertyShelves
{
    public abstract class ShelfPropertyDrawer : PropertyDrawer
    {
        protected ShelfGroup Group = new();

        public override void OnGUI(Rect rect, SerializedProperty property, GUIContent label)
        {
            Group.Render(rect, property, label);
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return Group.GetHeight(property, label);
        }

        protected T AddShelf<T>(T shelf) where T : IShelf
        {
            Group.AddShelf(shelf);
            return shelf;
        }
    }
}