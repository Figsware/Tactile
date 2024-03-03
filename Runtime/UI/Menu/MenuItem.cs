namespace Tactile.UI.Menu
{
    public class MenuItem
    {
        public readonly MenuStyle Style;

        public MenuItem(MenuStyle style)
        {
            Style = style;
        }
    }

    public class MenuAction : MenuItem
    {
        public readonly MenuState State;

        public MenuAction(MenuStyle style, MenuState state) : base(style)
        {
            State = state;
        }
    }

    public class MenuFolder : MenuItem
    {
        public MenuItem[] Items;

        public MenuFolder(MenuStyle style, MenuItem[] items) : base(style)
        {
            Items = items;
        }
    }
}