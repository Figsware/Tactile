using UnityEditor;
using UnityEngine;

namespace Tactile.Editor.Utility.PropertyShelves
{
    public class Shelf: IShelf
    {
        private readonly IShelf.ShelfRenderer _renderer;
        private readonly IShelf.ShelfHeightCalculator _heightCalculator;

        public Shelf(IShelf.ShelfRenderer renderer, IShelf.ShelfHeightCalculator heightCalculator)
        {
            _renderer = renderer;
            _heightCalculator = heightCalculator;
        }

        public Shelf(IShelf.ShelfRenderer renderer)
        {
            _renderer = renderer;
            _heightCalculator = (props, label) => EditorGUIUtility.singleLineHeight;
        }

        public void Render(Rect rect, SerializedProperty property, GUIContent label) =>
            _renderer(rect, property, label);

        public float GetHeight(SerializedProperty property, GUIContent label) => _heightCalculator(property, label);
    }
}