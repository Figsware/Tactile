using Tactile.UI;
using Tactile.Utility;
using UnityEditor;
using UnityEngine;

namespace Tactile.Editor
{
    [CustomPropertyDrawer(typeof(UnityDictionary<,>.DictionaryItem))]
    public class UnityDictionaryItemPropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect rect, SerializedProperty property,
            GUIContent label)
        {
            var keyValueRects = rect.SplitRects(TactileGUI.SplitDirection.Horizontal, 8, 1, 2);
            var keyProp = GetKeyProperty(property);
            var valProp = GetValueProperty(property);
            EditorGUI.PropertyField(keyValueRects[0].OffsetPrefixLabelIndent(), keyProp, GUIContent.none, true);
            EditorGUI.PropertyField(keyValueRects[1].OffsetPrefixLabelIndent(), valProp, GUIContent.none, true);
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(GetValueProperty(property));
        }

        private SerializedProperty GetKeyProperty(SerializedProperty property) => property.FindPropertyRelative("key");

        private SerializedProperty GetValueProperty(SerializedProperty property) =>
            property.FindPropertyRelative("value");
    }
}