using UnityEditor;
using UnityEngine;

namespace Tactile.Editor.Utility.PropertyShelves
{
    public class PropertyShelf : IShelf
    {
        private readonly string _relativePropertyPath;
        public bool HideLabel = false;
        public bool IncludeChildren = true;
        
        public PropertyShelf(string relativePropertyPath)
        {
            _relativePropertyPath = relativePropertyPath;
        }
        
        public void Render(Rect rect, SerializedProperty property, GUIContent label)
        {
            if (HideLabel)
                EditorGUI.PropertyField(rect, GetPropertyField(property), GUIContent.none, IncludeChildren);
            else
                EditorGUI.PropertyField(rect, GetPropertyField(property), IncludeChildren);
        }

        public float GetHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(GetPropertyField(property));
        }

        private SerializedProperty GetPropertyField(SerializedProperty property) => property.FindPropertyRelative(_relativePropertyPath);
    }
}