namespace Tactile.Utility.Logging
{
    public interface INavigator<T>
    {
        int Count { get; }
        T Pop();
        T PopToFirstItem();
        void Push(T item);
        T Peek();
    }
}