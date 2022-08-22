using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Linq;

namespace Tactile.Editor
{

    /// <summary>
    /// Allows editing of a template item. This will provide an interface to edit the value of a template item and choose
    /// whether to use a fallback or not.
    /// </summary>
    [CustomPropertyDrawer(typeof(TemplateItem<>.Descriptor))]
    public class TemplateItemDescriptorDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            float spacing = 2f;
            float contentWidth = position.width - spacing;
            float keyWidth = contentWidth / 4f;
            float valueWidth = 3f * contentWidth / 4f;
            SerializedProperty key = property.FindPropertyRelative("key");
            SerializedProperty value = property.FindPropertyRelative("value");
            Rect valueRect = new Rect(position.x + spacing + keyWidth, position.y, valueWidth,
                position.height);
            EditorGUI.PropertyField(new Rect(position.x, position.y, keyWidth, position.height), key, GUIContent.none);
            if (value != null)
                EditorGUI.PropertyField(valueRect, value);
            else
                EditorGUI.HelpBox(valueRect, "Non-serializable property", MessageType.Error);
            EditorGUI.EndProperty();
        }
    }
}