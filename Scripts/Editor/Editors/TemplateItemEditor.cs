using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


namespace Tactile.Editor
{

    /// <summary>
    /// Allows editing of a template item. This will provide an interface to edit the value of a template item and choose
    /// whether to use a fallback or not.
    /// </summary>
    [CustomPropertyDrawer(typeof(TemplateItem<>))]
    public class TemplateItemDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            SerializedProperty fallbackprop = property.FindPropertyRelative("fallbackValue");
            EditorGUI.PropertyField(position, fallbackprop, label, true);
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property.FindPropertyRelative("fallbackValue"));
        }
    }
}

