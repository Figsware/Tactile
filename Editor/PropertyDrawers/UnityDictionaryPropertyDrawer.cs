using System;
using Tactile.Utility;
using Tactile.UI;
using UnityEditor;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;

namespace Tactile.Editor
{
    [CustomPropertyDrawer(typeof(UnityDictionary<,>))]
    public class UnityDictionaryPropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(UnityEngine.Rect rect, SerializedProperty property,
            UnityEngine.GUIContent label)
        {
           // rect = EditorGUI.PrefixLabel(rect, label);
           var items = GetItemsProperty(property);
           EditorGUI.PropertyField(rect, items, label);
        }

        public override float GetPropertyHeight(SerializedProperty property, UnityEngine.GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(GetItemsProperty(property));
        }
        
        private SerializedProperty GetItemsProperty(SerializedProperty property) => property.FindPropertyRelative("items");
       
    }
}

