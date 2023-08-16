using Tactile.UI;
using Tactile.Utility;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace Tactile.Editor.CustomEditors
{
    // [CustomPropertyDrawer(typeof(SelectableList<>), true)]
    public class SelectableListPropertyDrawer : PropertyDrawer
    {
        private static Texture _nullImage;
        private static Texture _valueImage;
        
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // // position = EditorGUI.PrefixLabel(position, label);
            //
            // var reorderable = new ReorderableList();
            // var list = property.FindPropertyRelative("list");
            // EditorGUI.PropertyField(position, list);
            // // EditorGUI.
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property.FindPropertyRelative("list"));
        }
    }
}