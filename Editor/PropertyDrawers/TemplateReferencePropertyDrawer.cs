using Tactile.UI;
using Tactile.Utility;
using Tactile.Utility.Templates;
using UnityEditor;
using UnityEngine;

namespace Tactile.Editor.PropertyDrawers
{
    [CustomPropertyDrawer(typeof(Template<>.Reference), true)]
    public class TemplateReferencePropertyDrawer : PropertyDrawer
    {
        private const string UsesKeyPropertyName = "usesKey";
        private const string KeyPropertyName = "key";
        private const string FallbackValuePropertyName = "fallbackValue";

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            position = EditorGUI.PrefixLabel(position, label);

            var rects = position.HorizontalLayout(4f, RectLayout.SingleLineHeight, RectLayout.Flex());
            var usesKeyProperty = property.FindPropertyRelative(UsesKeyPropertyName);
            var keyProperty = property.FindPropertyRelative(KeyPropertyName);
            var fallbackValueProperty = property.FindPropertyRelative(FallbackValuePropertyName);
            var usesKey = usesKeyProperty.boolValue;

            EditorGUI.PropertyField(rects[1].OffsetPrefixLabelIndent(), usesKey ? keyProperty : fallbackValueProperty,
                GUIContent.none);

            var style = new GUIStyle(GUI.skin.button);
            style.padding.bottom = 2;
            style.padding.top = 2;
            style.padding.left = 2;
            style.padding.right = 2;

            if (GUI.Button(rects[0],
                    TactileEditorTextures.GetTexture(usesKey ? TactileEditorTextures.Key : TactileEditorTextures.NoKey),
                    style))
                usesKeyProperty.boolValue = !usesKey;
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var hasValueProperty = property.FindPropertyRelative(UsesKeyPropertyName);
            var hasValue = hasValueProperty.boolValue;

            return hasValue
                ? EditorGUI.GetPropertyHeight(property.FindPropertyRelative(FallbackValuePropertyName))
                : EditorGUIUtility.singleLineHeight;
        }
    }
}