using Tactile.UI;
using Tactile.Utility;
using UnityEditor;
using UnityEngine;

namespace Tactile.Editor.CustomEditors
{
    [CustomPropertyDrawer(typeof(UnityNullable<>), true)]
    public class UnityNullablePropertyDrawer : PropertyDrawer
    {
        private static Texture _nullImage;
        private static Texture _valueImage;
        
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            position = EditorGUI.PrefixLabel(position, label);

            var rects = position.SplitRects(TactileGUI.SplitDirection.Horizontal, 4, -EditorGUIUtility.singleLineHeight,
                1f);
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

            if (!(_nullImage || _valueImage))
            {
                _nullImage = GetNullableIcon("null");
                _valueImage = GetNullableIcon("value");
            }

            var style = new GUIStyle(GUI.skin.button);
            style.padding.bottom = 2;
            style.padding.top = 2;
            style.padding.left = 2;
            style.padding.right = 2;

            if (GUI.Button(rects[0], hasValue ? _valueImage : _nullImage, style))
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
        
        private Texture2D GetNullableIcon(string iconName)
        {
            return TactileUtility.TextureFromPNG($"Packages/com.figsware.tactile/Icons/Nullable/{iconName}.png");
        }
    }
}