namespace Tactile.UI.Menu
{
    public class MenuObject
    {
        public readonly MenuStyle Style;

        public MenuObject(MenuStyle style)
        {
            Style = style;
        }
    }

    public class MenuItem : MenuObject
    {
        public readonly MenuState State;

        public MenuItem(MenuStyle style, MenuState state) : base(style)
        {
            State = state;
        }
    }

    public class MenuFolder : MenuObject
    {
        public readonly MenuObject[] Items;

        public MenuFolder(MenuStyle style, MenuObject[] items) : base(style)
        {
            Items = items;
        }
    }
}