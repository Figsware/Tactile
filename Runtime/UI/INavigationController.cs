namespace Tactile.UI
{
    public interface INavigationController
    {
        public string Title { get; }
        public int ItemCount { get; }
        public void GoBack();
        public void GoToInitialView();
    }
}