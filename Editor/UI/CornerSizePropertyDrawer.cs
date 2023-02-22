using Tactile.UI;
using UnityEditor;
using UnityEngine;

namespace Tactile.Editor.UI
{
    [CustomPropertyDrawer(typeof(Rectangle.CornerSizes))]
    public class CornerSizePropertyDrawer : PropertyDrawer
    {
        private readonly Vector2 rectangleMargin = new Vector2(10f, 10f);
        private readonly Vector2 inputFieldSize = new Vector2(60f, 20f);
        public const float RectangleHeight = 75f;
        private readonly Texture2D Background = TactileUtility.CreateSolidColorTexture(Color.gray);
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            float width = inputFieldSize.x;
            float height = inputFieldSize.y;
            var topLeft = property.FindPropertyRelative(nameof(Rectangle.CornerSizes.topLeft));
            var topRight = property.FindPropertyRelative(nameof(Rectangle.CornerSizes.topRight));
            var bottomLeft = property.FindPropertyRelative(nameof(Rectangle.CornerSizes.bottomLeft));
            var bottomRight = property.FindPropertyRelative(nameof(Rectangle.CornerSizes.bottomRight));

            EditorGUI.PropertyField(new Rect(
                position.x,
                position.y,
                inputFieldSize.x, inputFieldSize.y), topLeft, GUIContent.none);
            
            EditorGUI.PropertyField(new Rect(
                position.x + position.width - width,
                position.y,
                inputFieldSize.x, inputFieldSize.y), topRight, GUIContent.none);
            
            EditorGUI.PropertyField(new Rect(
                position.x,
                position.y + position.height - height,
                inputFieldSize.x, inputFieldSize.y), bottomLeft, GUIContent.none);
            
            EditorGUI.PropertyField(new Rect(
                position.x + position.width - width,
                position.y + position.height - height,
                inputFieldSize.x, inputFieldSize.y), bottomRight, GUIContent.none);
            
            GUI.DrawTexture(new Rect(
                position.x + width + rectangleMargin.x, 
                position.y + height + rectangleMargin.y, 
                position.width - 2 * (width + rectangleMargin.x), 
                position.height - 2 * (height + rectangleMargin.y)),
                Background);   
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return RectangleHeight + (inputFieldSize.y + rectangleMargin.y) * 2f;
        }
    }
}