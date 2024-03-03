using Tactile.UI.Menu;
using Tactile.Utility;
using UnityEditor;
using UnityEngine;

namespace Tactile.Editor.PropertyDrawers
{
    [CustomPropertyDrawer(typeof(UnityNullable<>), true)]
    public class UnityNullablePropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            position = EditorGUI.PrefixLabel(position, label);
            var rects = position.HorizontalLayout(4f, RectLayout.SingleLineHeight, RectLayout.Flex());
            var hasValueProperty = property.FindPropertyRelative("hasValue");
            var valueProperty = property.FindPropertyRelative("value");
            var hasValue = hasValueProperty.boolValue;

            if (hasValue)
            {
                EditorGUI.PropertyField(rects[1].OffsetPrefixLabelIndent(), valueProperty, GUIContent.none);
            }
            else
            {
                EditorGUI.LabelField(rects[1].OffsetPrefixLabelIndent(), "No value");
            }

            var style = new GUIStyle(GUI.skin.button);
            style.padding.bottom = 2;
            style.padding.top = 2;
            style.padding.left = 2;
            style.padding.right = 2;

            if (GUI.Button(rects[0],
                    TactileEditorTextures.GetTexture(hasValue
                        ? TactileEditorTextures.Value
                        : TactileEditorTextures.NullValue), style))
                hasValueProperty.boolValue = !hasValue;
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var hasValueProperty = property.FindPropertyRelative("hasValue");
            var hasValue = hasValueProperty.boolValue;

            return hasValue
                ? EditorGUI.GetPropertyHeight(property.FindPropertyRelative("value"))
                : EditorGUIUtility.singleLineHeight;
        }
    }
}