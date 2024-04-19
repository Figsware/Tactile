using Tactile.Utility.Logging;
using UnityEditor;
using UnityEngine;

namespace Tactile.Editor.PropertyDrawers
{
    [CustomPropertyDrawer(typeof(KeyReference<>), true)]
    public class KeyReferencePropertyDrawer : PropertyDrawer
    {
        private const string UsesKeyPropertyName = "usesKey";
        private const string KeyPropertyName = "key";
        private const string InlineValuePropertyName = "inlineValue";

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var usesKeyProperty = property.FindPropertyRelative(UsesKeyPropertyName);
            var keyProperty = property.FindPropertyRelative(KeyPropertyName);
            var fallbackValueProperty = property.FindPropertyRelative(InlineValuePropertyName);
            var usesKey = usesKeyProperty.boolValue;
            var isDropdownReference = fallbackValueProperty.propertyType == SerializedPropertyType.Generic;
            var usePrefixLabel = usesKey || !isDropdownReference;
         
            // position = usePrefixLabel ? EditorGUI.PrefixLabel(position, label) : position;
            position = EditorGUI.PrefixLabel(position, usePrefixLabel ? label : GUIContent.none);
            
            var rects = position.HorizontalLayout(4f, RectLayout.Flex(), RectLayout.SingleLineHeight);
            
            EditorGUI.PropertyField(rects[0].OffsetPrefixLabelIndent(), usesKey ? keyProperty : fallbackValueProperty,
                usePrefixLabel ? GUIContent.none : label, true);

            var style = new GUIStyle(GUI.skin.button);
            style.padding.bottom = 2;
            style.padding.top = 2;
            style.padding.left = 2;
            style.padding.right = 2;

            if (GUI.Button(rects[1].VerticalLayout(0f, RectLayout.SingleLineHeight)[0],
                    TactileEditorTextures.GetTexture(usesKey ? TactileEditorTextures.Key : TactileEditorTextures.NoKey),
                    style))
                usesKeyProperty.boolValue = !usesKey;
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var usingKeyProperty = property.FindPropertyRelative(UsesKeyPropertyName);
            var usingKey = usingKeyProperty.boolValue;

            return usingKey
                ? EditorGUIUtility.singleLineHeight
                : EditorGUI.GetPropertyHeight(property.FindPropertyRelative(InlineValuePropertyName), true);
        }
    }
}