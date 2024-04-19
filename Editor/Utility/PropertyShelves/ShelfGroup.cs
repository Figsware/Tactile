using System.Collections.Generic;
using System.Linq;
using Tactile.Utility.Logging;
using UnityEditor;
using UnityEngine;

namespace Tactile.Editor.Utility.PropertyShelves
{
    public class ShelfGroup : IShelf
    {
        public float Gap = RectLayout.DefaultGap;
        private readonly List<ShelfWithHeight> _shelves = new();

        public ShelfGroup()
        {
            
        }

        public ShelfGroup(IEnumerable<IShelf> shelves)
        {
            _shelves.AddRange(shelves.Select(s => new ShelfWithHeight(s)));
        }
        
        public ShelfGroup(params IShelf[] shelves)
        {
            _shelves.AddRange(shelves.Select(s => new ShelfWithHeight(s)));
        }

        public void AddShelf(IShelf shelf)
        {
            _shelves.Add(new ShelfWithHeight(shelf));
        }

        public void Render(Rect rect, SerializedProperty property, GUIContent label)
        {
            var rects = rect.VerticalLayout(Gap,
                _shelves.Select(s => RectLayout.Size(s.LastHeight
                    )).Cast<RectLayout>().ToArray());

            for (var i = 0; i < _shelves.Count; i++)
            {
                var shelfRect = rects[i];
                var controller = _shelves[i];
                
                controller.Render(shelfRect, property, new GUIContent(label));
            }
        }

        public float GetHeight(SerializedProperty property, GUIContent label)
        {
            var totalShelfHeight =
                _shelves.Aggregate(0f, (h, controller) => h + controller.GetHeight(property, label));
            var totalSpacing = Gap * (_shelves.Count - 1);
            var totalHeight = totalShelfHeight + totalSpacing;

            return totalHeight;
        }

        private class ShelfWithHeight: IShelf
        {
            private readonly IShelf _shelf;
            public float LastHeight { get; private set; }

            public ShelfWithHeight(IShelf shelf)
            {
                _shelf = shelf;
            }

            public void Render(Rect rect, SerializedProperty property, GUIContent label) =>
                _shelf.Render(rect, property, label);

            public float GetHeight(SerializedProperty property, GUIContent label)
            {
                LastHeight = _shelf.GetHeight(property, label);
                return LastHeight;
            }
        }
    }
}