using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace Tactile.Editor.Utility.PropertyShelves
{
    public class ReorderableListShelf: IShelf
    {
        public readonly ReorderableList List;

        public ReorderableListShelf(ReorderableList list)
        {
            List = list;
        }
        
        public void Render(Rect rect, SerializedProperty property, GUIContent label)
        {
            List.DoList(rect);
        }

        public float GetHeight(SerializedProperty property, GUIContent label)
        {
            return List.GetHeight();
        }
    }
}