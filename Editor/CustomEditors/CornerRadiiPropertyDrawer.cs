using Tactile.UI;
using UnityEditor;
using UnityEngine;

namespace Tactile.Editor.CustomEditors
{
    [CustomPropertyDrawer(typeof(Rectangle.CornerRadii))]
    public class CornerRadiiPropertyDrawer : PropertyDrawer
    {
        private static Texture _topLeftImage;
        private static Texture _topRightImage;
        private static Texture _bottomLeftImage;
        private static Texture _bottomRightImage;
        private static Texture _noneSelectedImage;
        
        
        
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (!(_topLeftImage || _topRightImage || _bottomLeftImage || _bottomRightImage || _noneSelectedImage))
            {
                _topLeftImage = GetBorderRadiusIcon("border-radius-top-left");
                _topRightImage = GetBorderRadiusIcon("border-radius-top-right");
                _bottomLeftImage = GetBorderRadiusIcon("border-radius-bottom-left");
                _bottomRightImage = GetBorderRadiusIcon("border-radius-bottom-right");
                _noneSelectedImage = GetBorderRadiusIcon("border-radius-none-selected");
            }
            
            var topLeft = property.FindPropertyRelative(nameof(Rectangle.CornerRadii.topLeft));
            var topRight = property.FindPropertyRelative(nameof(Rectangle.CornerRadii.topRight));
            var bottomLeft = property.FindPropertyRelative(nameof(Rectangle.CornerRadii.bottomLeft));
            var bottomRight = property.FindPropertyRelative(nameof(Rectangle.CornerRadii.bottomRight));

            position = EditorGUI.PrefixLabel(position, label);
            
            var rects = position.SplitRects(TactileGUI.SplitDirection.Horizontal, 4, -EditorGUIUtility.singleLineHeight, 1, 1f, 1f, 1f);


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
            

            Texture borderRadiusImage = _noneSelectedImage;
            var currentSelected = GUI.GetNameOfFocusedControl();
            if (currentSelected == topLeftName)
            {
                borderRadiusImage = _topLeftImage;
            }
            else if (currentSelected == topRightName)
            {
                borderRadiusImage = _topRightImage;
            }
            else if (currentSelected == bottomLeftName)
            {
                borderRadiusImage = _bottomLeftImage;
            }
            else if (currentSelected == bottomRightName)
            {
                borderRadiusImage = _bottomRightImage;
            }

            GUI.DrawTexture(rects[0], borderRadiusImage, ScaleMode.StretchToFill);
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUIUtility.singleLineHeight;
        }

        private Texture2D GetBorderRadiusIcon(string iconName)
        {
            return TactileUtility.TextureFromPNG($"Packages/com.figsware.tactile/Icons/BorderRadius/{iconName}.png");
        }
    }
}