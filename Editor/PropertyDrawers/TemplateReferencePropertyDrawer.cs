﻿using Tactile.UI;
using Tactile.Utility;
using Tactile.Utility.Templates;
using UnityEditor;
using UnityEngine;

namespace Tactile.Editor.CustomEditors
{
    [CustomPropertyDrawer(typeof(Template<>.Reference), true)]
    public class TemplateReferencePropertyDrawer : PropertyDrawer
    {
        private static Texture _noKeyImage;
        private static Texture _keyImage;

        private const string UsesKeyPropertyName = "usesKey";
        private const string KeyPropertyName = "key";
        private const string FallbackValuePropertyName = "fallbackValue";
        
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            position = EditorGUI.PrefixLabel(position, label);

            var rects = position.SplitRects(TactileGUI.SplitDirection.Horizontal, 4, -EditorGUIUtility.singleLineHeight,
                1f);

            var usesKeyProperty = property.FindPropertyRelative(UsesKeyPropertyName);
            var keyProperty = property.FindPropertyRelative(KeyPropertyName);
            var fallbackValueProperty = property.FindPropertyRelative(FallbackValuePropertyName);
            var usesKey = usesKeyProperty.boolValue;

            EditorGUI.PropertyField(rects[1].OffsetPrefixLabelIndent(), usesKey ? keyProperty : fallbackValueProperty, GUIContent.none);

            if (!(_noKeyImage || _keyImage))
            {
                _noKeyImage = GetTemplateItemIcon("no_key");
                _keyImage = GetTemplateItemIcon("key");
            }
            
            var style = new GUIStyle(GUI.skin.button);
            style.padding.bottom = 2;
            style.padding.top = 2;
            style.padding.left = 2;
            style.padding.right = 2;
            
            if (GUI.Button(rects[0], usesKey ? _keyImage : _noKeyImage, style))
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

        private Texture2D GetTemplateItemIcon(string iconName)
        {
            return TactileUtility.TextureFromPNG($"Packages/com.figsware.tactile/Icons/TemplateItem/{iconName}.png");
        }
    }
}