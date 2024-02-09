using UnityEditor;
using UnityEngine;

namespace Tactile.Editor.Utility.PropertyShelves
{
    public interface IShelf
    {
        public delegate void ShelfRenderer(Rect rect, SerializedProperty property, GUIContent label);
        public delegate float ShelfHeightCalculator(SerializedProperty property, GUIContent label);
        
        public void Render(Rect rect, SerializedProperty property, GUIContent label);
        public float GetHeight(SerializedProperty property, GUIContent label);
    }
}