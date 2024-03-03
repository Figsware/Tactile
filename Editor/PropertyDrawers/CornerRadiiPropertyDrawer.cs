using Tactile.UI;
using Tactile.Utility;
using UnityEditor;
using UnityEngine;

namespace Tactile.Editor.PropertyDrawers
{
    [CustomPropertyDrawer(typeof(Rectangle.CornerRadii))]
    public class CornerRadiiPropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var topLeft = property.FindPropertyRelative(nameof(Rectangle.CornerRadii.topLeft));
            var topRight = property.FindPropertyRelative(nameof(Rectangle.CornerRadii.topRight));
            var bottomLeft = property.FindPropertyRelative(nameof(Rectangle.CornerRadii.bottomLeft));
            var bottomRight = property.FindPropertyRelative(nameof(Rectangle.CornerRadii.bottomRight));

            position = EditorGUI.PrefixLabel(position, label);
            
            var rects = position.HorizontalLayout(RectLayout.SingleLineHeight,
                RectLayout.Repeat(4, RectLayout.Flex()));

            var topLeftName = $"{property.name}-topLeft";
            var topRightName = $"{property.name}-topRight";
            var bottomLeftName = $"{property.name}-bottomLeft";
            var bottomRightName = $"{property.name}-bottomRight";

            GUI.SetNextControlName(topLeftName);
            EditorGUI.PropertyField(rects[1], topLeft, GUIContent.none);
            
            GUI.SetNextControlName(topRightName);
            EditorGUI.PropertyField(rects[2], topRight, GUIContent.none);
            
            GUI.SetNextControlName(bottomLeftName);
            EditorGUI.PropertyField(rects[3], bottomLeft, GUIContent.none);
            
            GUI.SetNextControlName(bottomRightName);
            EditorGUI.PropertyField(rects[4], bottomRight, GUIContent.none);
            
            
            var currentSelected = GUI.GetNameOfFocusedControl();
            var borderRadiusImage = TactileEditorTextures.GetTexture(currentSelected switch
            {
                _ when currentSelected == topLeftName => TactileEditorTextures.BorderRadiusTopLeft,
                _ when currentSelected == topRightName => TactileEditorTextures.BorderRadiusTopRight,
                _ when currentSelected == bottomLeftName => TactileEditorTextures.BorderRadiusBottomLeft,
                _ when currentSelected == bottomRightName => TactileEditorTextures.BorderRadiusBottomRight,
                _ => TactileEditorTextures.BorderRadiusNoneSelected
            });

            GUI.DrawTexture(rects[0], borderRadiusImage, ScaleMode.StretchToFill);
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUIUtility.singleLineHeight;
        }
    }
}