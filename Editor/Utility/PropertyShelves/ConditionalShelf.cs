using System;
using UnityEditor;
using UnityEngine;

namespace Tactile.Editor.Utility.PropertyShelves
{
    public class ConditionalShelf: IShelf
    {
        private bool _isRendering;
        private readonly Func<SerializedProperty, bool> _condition;
        private readonly IShelf _shelf;
        
        public ConditionalShelf(Func<SerializedProperty, bool> condition, IShelf shelf)
        {
            _condition = condition;
            _shelf = shelf;
        }

        public ConditionalShelf(string boolProperty, IShelf shelf, bool showWhenFalse = false)
        {
            _condition = prop => prop.FindPropertyRelative(boolProperty).boolValue;
            _shelf = shelf;
            
            // swf  prop
            //  T    T    F
            //  T    F    T
            //  F    T    T
            //  F    F    F
        }

        public void Render(Rect rect, SerializedProperty property, GUIContent label)
        {
            if (_isRendering)
                _shelf.Render(rect, property, label);
        }

        public float GetHeight(SerializedProperty property, GUIContent label)
        {
            _isRendering = _condition(property);
            return _isRendering ? _shelf.GetHeight(property, label) : 0;
        }
    }
}