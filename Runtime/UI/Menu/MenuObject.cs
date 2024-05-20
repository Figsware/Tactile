namespace Tactile.UI.Menu
{
    /// <summary>
    /// A base object for building a menu. Every MenuObject has a style that it can be stylized with.
    /// </summary>
    public class MenuObject
    {
        public readonly MenuStyle Style;

        public MenuObject(MenuStyle style)
        {
            Style = style;
        }
    }

    /// <summary>
    /// A menu item represents some action or setting within a menu. It does so by containing state
    /// </summary>
    public class MenuItem : MenuObject
    {
        public readonly MenuState State;

        public MenuItem(MenuStyle style, MenuState state) : base(style)
        {
            State = state;
        }
    }

    /// <summary>
    /// A menu folder represents a group of other menu items.
    /// </summary>
    public class MenuFolder : MenuObject
    {
        public readonly MenuObject[] Items;

        public MenuFolder(MenuStyle style, MenuObject[] items) : base(style)
        {
            Items = items;
        }
    }
}