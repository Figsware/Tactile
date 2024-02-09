using UnityEditor;
using UnityEngine;

namespace Tactile.Editor.Utility.PropertyShelves
{
    public class ShelfController: IShelf
    {
        public readonly IShelf Shelf;
        public bool Visible;
        
        public ShelfController(IShelf shelf, bool startingVisibility = true)
        {
            Shelf = shelf;
            Visible = startingVisibility;
        }
        
        public void Render(Rect rect, SerializedProperty property, GUIContent label)
        {
            if (Visible)
                Shelf.Render(rect, property, label);
        }

        public float GetHeight(SerializedProperty property, GUIContent label)
        {
            return Visible ? Shelf.GetHeight(property, label) : 0f;
        }

        public void ToggleVisible() => Visible = !Visible;
    }

    public class ShelfController<T>: ShelfController where T : IShelf
    {
        public T TShelf => (T)Shelf;
        
        public ShelfController(T shelf, bool startingVisibility = true) : base(shelf, startingVisibility)
        {
            
        }

        public static implicit operator T(ShelfController<T> shelfController) => shelfController.TShelf;
    }
}